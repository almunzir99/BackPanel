import { Component, OnInit } from '@angular/core';
import { Column } from 'src/app/shared/components/datatable/column.model';

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
  constructor() { }

  ngOnInit(): void {
  }

}
