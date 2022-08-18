import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ControlTypes } from './control-type.enum';
import { FormBuilderGroup } from './form-builder-group.model';

@Component({
  selector: 'form-builder',
  templateUrl: './form-builder.component.html',
  styleUrls: ['./form-builder.component.scss']
})
export class FormBuilderComponent implements OnInit {
  @Input("control-groups") controlsGroups: FormBuilderGroup[] = [];
  @Input("inner-form") innerForm: boolean = false;
  @Output("submit") submitEventEmitter = new EventEmitter<any>();
  @Output("cancel") cancelEventEmitter = new EventEmitter<void>();
  @Output("tableDelete") tableDeleteEvent = new EventEmitter<any>();
  formGroup: FormGroup = new FormGroup({});
  controlTypes = ControlTypes;
  constructor() {

  }
  onSubmit() {
    this.submitEventEmitter.emit(this.formGroup.value);
  }
  onCancel() {
    this.cancelEventEmitter.emit();
  }
  ngAfterContentInit() {
    this.controlsGroups.forEach(group => {
      group.controls.forEach(control => {
        if(control.name)
        this.formGroup.addControl(control.name, new FormControl(control.value, control.validators));
      });
    });
  }
  ngOnInit(): void {
  }
  
}

