import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';
import { FormBuilderComponent, FormBuilderPropsSpec } from 'src/app/shared/components/form-builder/form-builder.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  columns:Column[] =  [
     
      {
        prop: "id",
        title: "#",
        show: true,
        sortable: true

      },

      {
        prop: "username",
        title: "Username",
        show: true,
        sortable: true

      },
      {
        prop: "phone",
        title: "Phone Number",
        show: true,
        sortable: true


      },
      {
        prop: "email",
        title: "email",
        show: true,
        sortable: true


      },
      {
        prop: "role",
        title: "role",
        show: true,
        sortable:false,


      },
      {
        prop: "createdAt",
        title: "Created At",
        show: true,
        sortable: true


      },
      {
        prop: "lastUpdate",
        title: "Last Update",
        show: true,
        sortable: true


      }, {
        prop: "Actions",
        title: "Actions",
        show: false,
        sortable:false

      }
    ]

  data = [
    {
      "isManager": false,
      "activities": [],
      "image": null,
      "roleId": 4,
      "role": {
        "title": "SuperUser 2",
        "messagesPermissions": null,
        "adminsPermissions": null,
        "rolesPermissions": null,
        "id": 4,
        "createdAt": "0001-01-01  00:00:00",
        "lastUpdate": "2022-08-04  20:07:15",
        "createdBy": null
      },
      "username": "kalochhh",
      "email": "pikex@mailinator.com",
      "phone": "0915115763",
      "password": null,
      "photo": null,
      "token": null,
      "notifications": [],
      "id": 10,
      "createdAt": "2022-08-19  14:24:58",
      "lastUpdate": "2022-08-19  14:24:58",
      "createdBy": null
    },
    {
      "isManager": false,
      "activities": [],
      "image": null,
      "roleId": 6,
      "role": {
        "title": "super",
        "messagesPermissions": null,
        "adminsPermissions": null,
        "rolesPermissions": null,
        "id": 6,
        "createdAt": "2022-08-04  20:13:24",
        "lastUpdate": "2022-08-09  05:03:25",
        "createdBy": null
      },
      "username": "tejigase",
      "email": "cyciki@mailinator.com",
      "phone": "0128647017",
      "password": null,
      "photo": null,
      "token": null,
      "notifications": [],
      "id": 9,
      "createdAt": "2022-08-19  14:24:27",
      "lastUpdate": "2022-08-19  14:24:27",
      "createdBy": null
    },
    {
      "isManager": false,
      "activities": [
        {
          "id": 45,
          "userId": 6,
          "effectedTable": "Role",
          "effectedRowId": 6,
          "action": "Update",
          "createdAt": "2022-08-04  20:28:46"
        }
      ],
      "image": null,
      "roleId": 6,
      "role": {
        "title": "super",
        "messagesPermissions": null,
        "adminsPermissions": null,
        "rolesPermissions": null,
        "id": 6,
        "createdAt": "2022-08-04  20:13:24",
        "lastUpdate": "2022-08-09  05:03:25",
        "createdBy": null
      },
      "username": "Omer Hassan 2",
      "email": "maze0099@gmail.com",
      "phone": "0915115763",
      "password": null,
      "photo": null,
      "token": null,
      "notifications": [],
      "id": 6,
      "createdAt": "2022-08-01  21:50:52",
      "lastUpdate": "2022-08-04  20:14:17",
      "createdBy": null
    }
  ];
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
  create(){
    this.dialog.open<FormBuilderComponent,FormBuilderPropsSpec,any>(FormBuilderComponent,{
      data : {
      title:"Create New User",
        controlsGroups: this.form,
        onSubmit(result) {
          console.log(result);
        },
        onCancel(){

        }
      },
      panelClass:"form-builder-dialog",
    })
  }
  constructor(private dialog:MatDialog) {

   }

  ngOnInit(): void {
  }

}
