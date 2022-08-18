import { Validators } from "@angular/forms";
import { ControlTypes } from "./control-type.enum";
import { FormBuilderGroup } from "./form-builder-group.model";

export class FormBuilderControl {
        title?: string;
        name?: string;
        icon?:string;
        controlType?: ControlTypes;
        width?:string = "100%";
        alignRight?:boolean = false;
        data?:any[] = [];
        value?:any | any[]; 
        validators?:Validators = [];  
        //required for selection
        isObjectData?:boolean = false;
        labelProp?:string;
        valueProp?:string;
        // required for table builder
        controls?:FormBuilderGroup[]


} 