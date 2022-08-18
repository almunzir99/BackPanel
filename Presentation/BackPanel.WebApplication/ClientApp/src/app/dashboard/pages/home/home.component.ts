import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

   form: FormBuilderGroup[] = [
    {

      title: "General",
      controls: [
        {
          title: "id",
          name: "id",
          controlType: ControlTypes.Hidden,
          width: "0px",
          value: 1
        },
        {
          title: "username",
          name: "username",
          icon: "user tie icon",
          controlType: ControlTypes.TextInput,
          width: "50%",
          value: "almunzir alhassan",
          validators: [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(25),
          ]
        },
        {
          title: "phone",
          name: "phone",
          icon: "phone icon",
          controlType: ControlTypes.NumberInput,
          width: "50%",
          value: "0128647018",
          validators: [
            Validators.required,
            Validators.minLength(10),
            Validators.maxLength(12),

          ]

        },
        {
          title: "email",
          name: "email",
          icon: "mail icon",
          controlType: ControlTypes.TextInput,
          width: "100%",
          value: "almunzir alhassan",
          validators: [
            Validators.required

          ]

        },
        {
          title: "password",
          name: "password",
          icon: "key icon",

          controlType: ControlTypes.PasswordInput,
          width: "50%",
          validators: [
            Validators.required
          ]

        },
        {
          title: "re-enter the password",
          name: "repassword",
          icon: "key icon",
          validators: [
            Validators.required
          ],

          controlType: ControlTypes.PasswordInput,
          width: "50%"
        },
        {
          title: "Role",
          name: "roleId",
          controlType: ControlTypes.MultiSelection,
          data: [
            {
              "title":"super user",
              "id":1
            },
            {
              "title":"super sayian",
              "id":2
            }
          ],
          isObjectData: true,
          labelProp: "title",
          valueProp: "id",
          width: "100%",
          value: [1,2]

        },
        {
          title: "image",
          name: "image",
          controlType: ControlTypes.LocalFilePicker,
          width: "100%",
        },
        {
          title: "are you on Board ?",
          name: "agree",
          controlType: ControlTypes.CheckBox,
          width: "100%",
        },
        {
          title: "BirthDay",
          name: "birthDay",
          controlType: ControlTypes.DatePicker,
          width: "100%",
        },
        {
          title: "content",
          name: "content",
          controlType: ControlTypes.RichTextEditor,
          width: "100%",
          value:"you fucking fuelu"
        },
        


      ]
    }
  ];
  constructor() { }

  ngOnInit(): void {
  }

}
