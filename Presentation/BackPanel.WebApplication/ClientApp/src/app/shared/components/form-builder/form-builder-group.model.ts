import { ValidatorFn } from "@angular/forms";
import { FormBuilderControl } from "./form-builder-control.model";

export class FormBuilderGroup{
    title?:string;
    controls:FormBuilderControl[] = [];
    hidden?:boolean;
    Validators?:ValidatorFn[]
    
} 