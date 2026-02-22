import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { FileModel } from 'src/app/core/models/file.models';
import { ControlTypes } from './control-type.enum';
import { FormBuilderGroup } from './form-builder-group.model';
import { FormBuilderControl } from './form-builder-control.model';
import { TranslateService } from '@ngx-translate/core';
import { Direction } from '@angular/cdk/bidi';
import { MatRadioChange } from '@angular/material/radio';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { AlertMessageComponent, AlertMessage, MessageTypes } from '../alert-message/alert-message.component';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'form-builder',
  templateUrl: './form-builder.component.html',
  standalone: false,
  styleUrls: ['./form-builder.component.scss']
})
export class FormBuilderComponent implements OnInit, OnChanges {


  @Input("control-groups") controlsGroups: FormBuilderGroup[] = [];
  @Input("inner-form") innerForm: boolean = false;
  @Input("title") title?: string = '';
  @Input("confirm-button-text") confirmButtonText?: string = 'Dashboad.Save';
  @Input('show-cancel-button') showCancelButton = true;
  @Input('show-save-button') showSaveButton = true;
  @Output("formSubmit") submitEventEmitter = new EventEmitter<any>();
  @Output("cancel") cancelEventEmitter = new EventEmitter<void>();
  @Output("tableDelete") tableDeleteEvent = new EventEmitter<any>();
  @Output("onChangeWithComponentInfo") onChangeWithComponentInfo = new EventEmitter<any>();

  dir: Direction | "auto" = 'rtl'

  formGroup: FormGroup = new FormGroup({});
  controlTypes = ControlTypes;
  constructor(@Inject(MAT_DIALOG_DATA) public data: FormBuilderPropsSpec, 
  @Inject('DIRECTION') public direction: string, private _translateService: TranslateService, private _dialog: MatDialog,private datePipe: DatePipe) {
    if (data) {
      if (data.controlsGroups) this.controlsGroups = data.controlsGroups;
      this.title = data.title;
      this.confirmButtonText = data.saveButtonText ?? 'Dashboad.Save'
    }
    this.dir = _translateService.currentLang == 'ar' ? 'rtl' : 'ltr'

    _translateService.onLangChange.subscribe(res => {
      this.dir = res.lang == 'ar' ? 'rtl' : 'ltr'
    }, err => {

    })
  }

  onSubmit() {
    this.submitEventEmitter.emit(this.formGroup!.getRawValue());
    if (this.data) {
      this.data.onSubmit(this.formGroup!.getRawValue());
    }
  }
  onCancel(e: any) {
    e.preventDefault();
    this.cancelEventEmitter.emit();
    console.log(this.formGroup);
    if (this.data) {
      this.data.onCancel();
    }
  }
  // local files Picker Event
  onFilesPicked(name: string, files: FileModel[]) {
    var target = this.formGroup!.controls[name];
    target.setValue(files);

  }
  ngOnInit(): void {
    this.formGroup = new FormGroup({});
    this.controlsGroups.filter(x => !x.hidden).forEach(group => {
      group.controls.forEach(control => {
        if( control.controlType == ControlTypes.DatePicker && control.value){
          console.log(control.value);
          // format date to yyyy-MM-dd
          control.value = this.datePipe.transform(control.value, 'yyyy-MM-dd');
          console.log(control.value);
        }
        if (control.name)
          this.formGroup!.addControl(control.name, new FormControl({ value: control.value, disabled: control.disabled }, control.validators, control.asyncValidators));

      });
      if (group.Validators)
        group.Validators.forEach(val => {
          this.formGroup?.addValidators(val)
        });
    });
  }
  ngAfterViewInit() {
  }
  onSelectionValueChanged($event: any, control: FormBuilderControl) {
    if (control.onChange) {
      control.onChange($event);
      // update componen
    }
    if (control.onChangeWithUpdate) {
      control.onChangeWithUpdate($event, this.formGroup!)
    }
    if (control.onChangeWithComponentInfo) {
      control.onChangeWithComponentInfo($event, this)
    }
  }
  onInput($event: any, control: FormBuilderControl) {
    console.log($event)
    if (control.onChange) {
      control.onChange($event.target.value);
      // update component

    }
    if (control.onChangeWithUpdate) {
      control.onChangeWithUpdate($event.target.value, this.formGroup!)
    }
  }
  onDateChange($event: MatDatepickerInputEvent<any, any>, control: FormBuilderControl) {
    var value = $event.value;
    if (control.onChange) {
      control.onChange(value);
      // update component

    }
    if (control.onChangeWithUpdate) {
      control.onChangeWithUpdate(value, this.formGroup!)
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // check changes on controlsGroups
    if (changes.controlsGroups) {
      this.controlsGroups = changes.controlsGroups.currentValue;
      this.formGroup = new FormGroup({});
      this.controlsGroups.filter(c => !c.hidden).forEach(group => {
        group.controls.forEach(control => {
          if (control.name)
            this.formGroup!.addControl(control.name, new FormControl({ value: control.value, disabled: control.disabled }, control.controlType == ControlTypes.Hidden ? [] : control.validators));
        });
      });
    }
  }
  onRadioChanged($event: MatRadioChange, control: FormBuilderControl) {
    if (control.onChangeWithUpdate) {
      control.onChangeWithUpdate($event.value, this.formGroup!)
    }
    if (control.onChangeWithComponentInfo) {
      control.onChangeWithComponentInfo($event.value, this)
    }

  }

  getFileValue(arg0: any): string | null {
    return !arg0 ? null : Object.prototype.toString.call(arg0) === "[object String]" ? arg0 : arg0.path;
  }
  onLostFocus($event: any, control: FormBuilderControl) {
    if (control.onLostFocus) {
      control.onLostFocus($event.target.value);
      // update component

    }
  }
  onAutoCompleteChanged($event: any, control: FormBuilderControl) {
    if (control.onChange)
      control.onChange($event.target.value)
    control.filterData = control.data?.filter(c => c.includes($event.target.value))
  }

  // Sub Form Builder Methods
  onAddSubFormItem(control: FormBuilderControl) {
    if (!control.subFormItems) {
      control.subFormItems = [];
    }

    // Open a dialog or inline form to add new item
    this.openSubFormDialog(control, null, (result) => {
      control.subFormItems!.push(result);
      this.updateSubFormValue(control);
    });
  }

  onEditSubFormItem(control: FormBuilderControl, index: number) {
    const itemToEdit = control.subFormItems![index];

    // Open a dialog or inline form to edit existing item
    this.openSubFormDialog(control, itemToEdit, (result) => {
      control.subFormItems![index] = result;
      this.updateSubFormValue(control);
    });
  }

  onDeleteSubFormItem(control: FormBuilderControl, index: number) {
    // Show confirmation dialog before deletion
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: this._translateService.instant("Dashboard.CONFIRM_DELETE"),
        title: this._translateService.instant("Dashboard.CONFIRM")
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true) {
          control.subFormItems!.splice(index, 1);
          this.updateSubFormValue(control);
        }
      }
    })
  }

  private openSubFormDialog(control: FormBuilderControl, existingItem: any | null, onSave: (result: any) => void) {
    const dialogGroups = control.subFormGroups ?? [];

    if (existingItem) {
      dialogGroups.forEach((group: FormBuilderGroup) => {
        group.controls.forEach((dialogControl: FormBuilderControl) => {
          if (dialogControl.name && existingItem.hasOwnProperty(dialogControl.name)) {
            dialogControl.value = existingItem[dialogControl.name];
          }
        });
      });
    }
    dialogGroups.forEach((group: FormBuilderGroup) => {
      group.controls.forEach((dialogControl: FormBuilderControl) => {
        dialogControl.validators = dialogControl.validators?.filter((v: any) => v != null) || [];
      });
    });
    const dialogRef = this._dialog.open(FormBuilderComponent, {
      width: '600px',
      data: {
        title: existingItem ? (control.editButtonText ?? 'Dashboard.EDIT_ITEM') : (control.addButtonText ?? 'Dashboard.ADD_ITEM'),
        controlsGroups: dialogGroups,
        onSubmit: (result: any) => {
          onSave(result);
          dialogRef.close();
        },
        onCancel: () => {
          dialogRef.close();
        }
      }
    });
  }

  private updateSubFormValue(control: FormBuilderControl) {
    if (control.name) {
      this.formGroup.controls[control.name].setValue(control.subFormItems);
      this.formGroup.controls[control.name].markAsDirty();

      if (control.onChange) {
        control.onChange(control.subFormItems);
      }
      if (control.onChangeWithUpdate) {
        control.onChangeWithUpdate(control.subFormItems, this.formGroup);
      }
    }
  }
}


export interface FormBuilderPropsSpec {
  title?: string;
  saveButtonText?: string | null;
  controlsGroups: FormBuilderGroup[],
  onSubmit: (result: any) => void,
  onCancel: () => void
}