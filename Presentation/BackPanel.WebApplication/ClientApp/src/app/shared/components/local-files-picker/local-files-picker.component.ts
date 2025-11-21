import { Component, EventEmitter, Inject, Input, OnInit, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, UntypedFormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { FileModel } from 'src/app/core/models/file.models';
import { FilesManagerComponent, FilesManagerSpec } from 'src/app/dashboard/pages/files-manager/files-manager.component';

const CUSTOM_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => LocalFilesPickerComponent),
  multi: true,
};
@Component({
  selector: 'local-files-picker',
  templateUrl: './local-files-picker.component.html',
  standalone: false,
  styleUrls: ['./local-files-picker.component.scss'],
  providers: [CUSTOM_VALUE_ACCESSOR],
})
export class LocalFilesPickerComponent implements ControlValueAccessor, OnInit {
  inputTextContent = "general.selectedFiles";
  dir = 'ltr';
  @Output('filesPicked') filesPickedEventEmitter = new EventEmitter<FileModel[]>();
  @Input('formControlName') formControlName!: string;
  @Input('form-group') formGroup?: UntypedFormGroup;
  @Input('placeholder') placeholder?: string;
  @Input('initial-value') InitialValue: string | null = null;
  @Input('button-disabled') buttonDisabled: boolean = false;
  @Input("allow-mulitple-files") allowMultipleFiles = false;
  @Input('file-extensions') fileExtensions?: string | null = null;
  constructor(private dialog: MatDialog, 
    private translateService: TranslateService,
    @Inject("BASE_API_URL") public baseUrl: string,
  ) { 
    this.dir = this.translateService.currentLang == 'ar' ? 'rtl' : 'ltr';
    this.translateService.onLangChange.subscribe((event) => {
      this.dir = event.lang == 'ar' ? 'rtl' : 'ltr';
    })
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
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
    if (files.length > 0) {
      this.inputTextContent = `${files.length} ${this.translateService.instant('Dashboard.selectedFiles')}`;
      this.InitialValue = `${files.length} ${this.translateService.instant('Dashboard.selectedFiles')}`;

    }
    else
      this.inputTextContent = "Dashboard.selectedFiles";
    this.filesPickedEventEmitter.emit(files);
  }
  ngOnInit(): void {
  }

}