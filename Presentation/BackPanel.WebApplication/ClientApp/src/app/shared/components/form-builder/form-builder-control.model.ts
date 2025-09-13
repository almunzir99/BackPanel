import { AsyncValidator, FormGroup, Validators } from "@angular/forms";
import { ControlTypes } from "./control-type.enum";
import { FormBuilderGroup } from "./form-builder-group.model";
import { TemplateRef } from "@angular/core";
import { FormBuilderComponent } from "./form-builder.component";

export class FormBuilderControl {
     title?: string;
        name?: string;
        icon?:string;
        controlType?: ControlTypes;
        width?:string = "100%";
        alignRight?:boolean = false;
        data?:any[] = [];
        filterData?:any[] = [];
        value?:any | any[]; 
        validators?:Validators | any = [];  
        asyncValidators?:AsyncValidator | any = [];  

        disabled?:boolean = false;
        //required for selection
        isObjectData?:boolean = false;
        labelProp?:string;
        valueProp?:string;
        // required for table builder
        controls?:FormBuilderGroup[];
        // required for custom component
        template?:TemplateRef<any>;
        allowMultipleFile?:boolean;
        onChange?:(value:any) => void
        onLostFocus?:(value:any) => void
        onChangeWithUpdate?:(value:any,formGroup:FormGroup) => void
        onChangeWithComponentInfo?:(value:any,formBuilderComponent:FormBuilderComponent) => void



        displayWith?: ((value: any) => string)|null;
        extensions?: string


} 