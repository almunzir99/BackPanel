import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { FileModel } from 'src/app/core/models/file.models';
import { FilesManagerComponent, FilesManagerSpec } from 'src/app/dashboard/pages/files-manager/files-manager.component';

@Component({
  selector: 'local-files-picker',
  templateUrl: './local-files-picker.component.html',
  standalone: false,
  styleUrls: ['./local-files-picker.component.scss']
})
export class LocalFilesPickerComponent implements OnInit {
  inputTextContent = "Files picked here";
  @Output('filesPicked') filesPickedEventEmitter = new EventEmitter<FileModel[]>();
  dir = 'ltr';
  constructor(private dialog: MatDialog, private translate: TranslateService) {
    this.dir = this.translate.currentLang == 'ar' ? 'rtl' : 'ltr';
    this.translate.onLangChange.subscribe((event) => {
      this.dir = event.lang == 'ar' ? 'rtl' : 'ltr';
    })
  }
  onPickFiles() {
    this.dialog.open<FilesManagerComponent, FilesManagerSpec, any>(FilesManagerComponent, {
      data: {
        selectionMode: true,
        onFilesSubmitted: this.onFilesSubmitted

      },
      id: "form-builder-dialog",
      panelClass: 'dialog-container-bg'
    })
  }
  onFilesSubmitted = (files: FileModel[]) => {
    if (files.length > 0)
      this.inputTextContent = `${files.length} Files Loaded`;
    else
      this.inputTextContent = "Files picked here";
    this.filesPickedEventEmitter.emit(files);
  }
  ngOnInit(): void {
  }

}
