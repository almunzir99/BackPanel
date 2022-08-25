import { HttpEventType } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin, map, Subscription } from 'rxjs';
import { DirectoryModel } from 'src/app/core/models/directory.model';
import { FileModel } from 'src/app/core/models/file.models';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { FilesManagerService } from 'src/app/core/services/files-manager.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';
import { FormBuilderComponent, FormBuilderPropsSpec } from 'src/app/shared/components/form-builder/form-builder.component';
import { Clipboard } from '@angular/cdk/clipboard';
import * as dayjs from 'dayjs'

@Component({
  selector: 'app-files-manager',
  templateUrl: './files-manager.component.html',
  styleUrls: ['./files-manager.component.scss']
})
export class FilesManagerComponent implements OnInit {
  pathSegmentsStack: string[] = [];
  files: FileModel[] = [];
  directories: DirectoryModel[] = [];
  subscription: Subscription = new Subscription();
  getStatus = RequestStatus.Initial;
  createDirectoryStatus = RequestStatus.Initial;
  deleteStatus = RequestStatus.Initial;
  uploadFilesStatus = RequestStatus.Initial;
  renameStatus = RequestStatus.Initial;
  moveStatus = RequestStatus.Initial;
  progress = 0.0;
  currentMoveContentItem?: MoveContentSpec;
  selectionMode = false;
  selectedFiles: FileModel[] = [];
  constructor(
    private _service: FilesManagerService,
    private matDialog: MatDialog,
    private snackbar: MatSnackBar,
    private clipboard: Clipboard,
    @Inject(MAT_DIALOG_DATA) private data: FilesManagerSpec
  ) {
   }
  ngOnInit(): void {
    if(this.data)
    this.selectionMode = this.data.selectionMode;
    this.getDirectoriesAndFiles();
  }
  getDirectoriesAndFiles() {
    this.getStatus = RequestStatus.Loading;
    var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
    var obs = forkJoin([this._service.getDirectories(path), this._service.getFiles(path)])
      .pipe(map(([directories, files]) => {
        return { directories, files };
      }));
    var sub = obs.subscribe({
      next: (value) => {
        this.getStatus = RequestStatus.Success;
        this.files = value.files.data;
        this.directories = value.directories.data;
        console.log(this.directories);
      },
      error: (err) => {
        console.log(err);
        this.getStatus = RequestStatus.Failed;

      }
    })
    this.subscription.add(sub);
  }
  /************************ Events Bindings ******************************* */
  formatDate(date: string): string {
    return dayjs(date).format('DD MMM')
  }
  bytesToMB(size: number): string {
    var kb = size / 1024;
    var mb = kb / 1024;
    return mb.toFixed(2);

  }
  onSelectionChange(checked: boolean, file: FileModel) {
    if (checked) {
      if (!this.selectedFiles.includes(file))
        this.selectedFiles.push(file);
    }
    else {
      var index = this.selectedFiles.indexOf(file);
      this.selectedFiles.splice(index, 1);
    }
  }
  onSubmitClick() {
      if(this.data)
      this.data.onFilesSubmitted(this.selectedFiles)
      var dialog = this.matDialog.getDialogById("form-builder-dialog");
      if(dialog)
      dialog.close();
      
  }
  onCloseClick() {
    var dialog = this.matDialog.getDialogById("form-builder-dialog");
    if(dialog)
    dialog.close();
  }
  navigate(title: string) {
    this.pathSegmentsStack.push(title);
    this.getDirectoriesAndFiles();
  }
  navigateByLink(index: number) {
    if (index == -1)
      this.pathSegmentsStack = [];
    else
      this.pathSegmentsStack = this.pathSegmentsStack.slice(0, index + 1);
    this.getDirectoriesAndFiles();
  }
  openCreateDirectoryDialog() {
    var formControlsGroup: FormBuilderGroup[] = [
      {
        title: "Create New Directory",
        controls: [
          {
            title: "Directory Name",
            name: "name",
            controlType: ControlTypes.TextInput,
            icon: 'folder_open'
          }
        ]
      }
    ];
    this.matDialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        controlsGroups: formControlsGroup,
        onSubmit: (res) => {
          this.matDialog.closeAll();
          var name = res['name'];
          var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
          this.createDirectory(name, path);
        },
        onCancel: () => {
          this.matDialog.closeAll();
        },
      },
    })
  }
  openRenameDirectoryDialog(dirName: string) {
    var formControlsGroup: FormBuilderGroup[] = [
      {
        title: "Rename Directory",
        controls: [
          {
            title: "Directory Name",
            name: "name",
            controlType: ControlTypes.TextInput,
            icon: 'folder_open',
            value: dirName
          }
        ]
      }
    ];
    this.matDialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        controlsGroups: formControlsGroup,
        onSubmit: (res) => {
          this.matDialog.closeAll();
          var newName = res['name'];
          var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
          this.renameDirectory(dirName, newName, path);
        },
        onCancel: () => {
          this.matDialog.closeAll();
        },
      },
    })
  }

  onDirectoryDelete(dirName: string) {
    this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true) {
          var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
          this.deleteDirectory(dirName, path);
        }
      }
    })
  }
  onFilesLoaded(event: any) {
    var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
    this.uploadFiles(event.target.files, path);
  }
  onFileDelete(fileName: string) {
    this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true) {
          var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
          this.deleteFile(fileName, path);
        }
      }
    })
  }
  openRenameFileDialog(fileName: string) {
    var formControlsGroup: FormBuilderGroup[] = [
      {
        title: "Rename File",
        controls: [
          {
            title: "File Name",
            name: "name",
            controlType: ControlTypes.TextInput,
            icon: 'description',
            value: fileName
          }
        ]
      }
    ];
    this.matDialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        controlsGroups: formControlsGroup,
        onSubmit: (res) => {
          this.matDialog.closeAll();
          var newName = res['name'];
          var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
          this.renameDirectory(fileName, newName, path);
        },
        onCancel: () => {
          this.matDialog.closeAll();
        },
      },
    })
  }
  downloadFile(file: FileModel) {
    const link = document.createElement('a');
    link.setAttribute('target', '_blank');
    link.setAttribute('href', file.uri);
    link.setAttribute('download', file.title);
    link.click();
    link.remove();
  }
  copyLinkToClipBoard(link: string) {
    var result = this.clipboard.copy(link);
    if (result)
      this.snackbar.open("Link copied to clipboard", "Close", {
        duration: 2000,
        panelClass: ['snackbar']
      });
  }
  contentMove(name: string, type: string, action: string) {
    var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
    this.currentMoveContentItem = {
      type: type,
      name: name,
      oldPath: path,
      action: action

    }

  }
  contentPaste() {
    if (this.currentMoveContentItem) {
      var path = this.pathSegmentsStack.length == 0 ? undefined : this.pathSegmentsStack.join("/");
      this.moveContent(this.currentMoveContentItem.type, this.currentMoveContentItem.name, this.currentMoveContentItem.oldPath, path);
    }
  }


  /******************************* Actions ************************************** */

  createDirectory(name: string, path: string | undefined) {
    this.createDirectoryStatus = RequestStatus.Loading;
    this._service.createDirectory(name, path).subscribe({
      next: (_) => {
        this.createDirectoryStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();

      }, error: (err) => {
        console.log(err);
        this.createDirectoryStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })

  }
  renameDirectory(dirName: string, name: string, path: string | undefined) {
    this.renameStatus = RequestStatus.Loading;
    this._service.renameDir(dirName, name, path).subscribe({
      next: (_) => {
        this.renameStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();

      }, error: (err) => {
        console.log(err);
        this.renameStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })
  }
  deleteDirectory(name: string, path: string | undefined) {
    this.deleteStatus = RequestStatus.Loading;
    this._service.deleteDirectory(name, path).subscribe({
      next: (_) => {
        this.deleteStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();

      }, error: (err) => {
        console.log(err);
        this.deleteStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })
  }
  deleteFile(fileName: string, path: string | undefined) {
    this.deleteStatus = RequestStatus.Loading;
    this._service.deleteFile(fileName, path).subscribe({
      next: (_) => {
        this.deleteStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();

      }, error: (err) => {
        console.log(err);
        this.deleteStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })
  }
  renameFile(fileName: string, name: string, path: string | undefined) {
    this.renameStatus = RequestStatus.Loading;
    this._service.renameFile(fileName, name, path).subscribe({
      next: (_) => {
        this.renameStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();

      }, error: (err) => {
        console.log(err);
        this.renameStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })
  }
  moveContent(type: string, name: string, oldPath: string | undefined, newPath: string | undefined) {
    this.moveStatus = RequestStatus.Loading;
    var obs = type == "directory" ? this._service.moveDirectory(name, oldPath, newPath) : this._service.moveFile(name, oldPath, newPath);
    obs.subscribe({
      next: (_) => {
        this.moveStatus = RequestStatus.Success;
        this.getDirectoriesAndFiles();
        this.currentMoveContentItem = undefined;

      }, error: (err) => {
        console.log(err);
        this.currentMoveContentItem = undefined;
        this.moveStatus = RequestStatus.Failed;
        this.matDialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
          data: {
            type: MessageTypes.ERROR,
            message: "Sorry, Operation Failed Please try again",
            title: "Failed"
          }
        })
      }
    })
  }
  uploadFiles(files: File[], path?: string) {
    this.uploadFilesStatus = RequestStatus.Loading;
    var sub = this._service.uploadFiles(files, path).subscribe({
      next: (events) => {
        if (events.type === HttpEventType.UploadProgress) {
          if (events.total)
            this.progress = Math.round(100 * events.loaded / events.total);
        }
        if (this.progress == 100) {
          this.uploadFilesStatus = RequestStatus.Success;
          this.getDirectoriesAndFiles();
          this.progress = 0;
        }
      }, error: err => {
        console.log(err);
        this.uploadFilesStatus = RequestStatus.Failed;
      }
    })
    this.subscription.add(sub);

  }

}
export interface FilesManagerSpec {
  selectionMode: boolean;
  onFilesSubmitted: (files: FileModel[]) => void
}
export interface MoveContentSpec {
  oldPath?: string;
  name: string;
  type: string;
  action: string;
}