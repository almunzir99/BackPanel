import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { FileModel } from 'src/app/core/models/file.models';
import { ControlTypes } from './control-type.enum';
import { FormBuilderGroup } from './form-builder-group.model';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'form-builder',
  templateUrl: './form-builder.component.html',
  standalone: false,
  styleUrls: ['./form-builder.component.scss']
})
export class FormBuilderComponent implements OnInit {
  @Input("control-groups") controlsGroups: FormBuilderGroup[] = [];
  @Input("inner-form") innerForm: boolean = false;
  @Input("title") title?: string = '';
  @Input('show-cancel-button') showCancelButton = true;
  @Output("formSubmit") submitEventEmitter = new EventEmitter<any>();
  @Output("cancel") cancelEventEmitter = new EventEmitter<void>();
  @Output("tableDelete") tableDeleteEvent = new EventEmitter<any>();
  formGroup: UntypedFormGroup = new UntypedFormGroup({});
  controlTypes = ControlTypes;
  direction = 'ltr';
  constructor(@Inject(MAT_DIALOG_DATA) public data: FormBuilderPropsSpec, private translate: TranslateService) {
    if (data) {
      if (data.controlsGroups) this.controlsGroups = data.controlsGroups;
      this.title = data.title;
    }
    this.direction = this.translate.currentLang == 'ar' ? 'rtl' : 'ltr';
    this.translate.onLangChange.subscribe((event) => {
      this.direction = event.lang == 'ar' ? 'rtl' : 'ltr';
    })
  }
  onSubmit() {
    this.submitEventEmitter.emit(this.formGroup.value);
    if (this.data) {
      this.data.onSubmit(this.formGroup.value);
    }
  }
  onCancel(e: any) {
    e.preventDefault();
    this.cancelEventEmitter.emit();
    if (this.data) {
      this.data.onCancel();
    }
  }
  // local files Picker Event
  onFilesPicked(name: string, files: FileModel[]) {
    var target = this.formGroup.controls[name];
    target.setValue(files);

  }
  ngAfterContentInit() {
    this.controlsGroups.forEach(group => {
      group.controls.forEach(control => {
        if (control.name)
          this.formGroup.addControl(control.name, new FormControl(control.value, control.validators));
      });
    });
  }
  ngOnInit(): void {
  }
  getFileValue(arg0: any): string | null {
    return !arg0 ? null : Object.prototype.toString.call(arg0) === "[object String]" ? arg0 : arg0.path;
  }
}

export interface FormBuilderPropsSpec {
  title?: string;
  controlsGroups: FormBuilderGroup[],
  onSubmit: (result: any) => void,
  onCancel: () => void
}
