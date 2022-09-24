import { Component, Inject, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Admin } from 'src/app/core/models/admin.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { Role } from 'src/app/core/models/role.model';
import { AdminsService } from 'src/app/core/services/admins.service';
import { RolesService } from 'src/app/core/services/roles.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { PageSpec, SortSpec } from 'src/app/shared/components/datatable/datatable.component';
import { ControlTypes } from 'src/app/shared/components/form-builder/control-type.enum';
import { FormBuilderGroup } from 'src/app/shared/components/form-builder/form-builder-group.model';
import { FormBuilderComponent, FormBuilderPropsSpec } from 'src/app/shared/components/form-builder/form-builder.component';

@Component({
  selector: 'app-admins',
  templateUrl: './admins.component.html',
  styleUrls: ['./admins.component.scss']
})
export class AdminsComponent implements OnInit {
  columns: Column[] = [];
  data: Admin[] = [];
  pageIndex = 1;
  pageSize = 10;
  totalRecords = 0;
  totalPages = 1;
  orderBy = "lastUpdate";
  ascending = false;
  searchValue = "";
  getRequest = RequestStatus.Initial;
  dimRequest = RequestStatus.Initial;
  constructor(
    private _service: AdminsService, 
    private _rolesService: RolesService, 
    private _dialog: MatDialog,
    @Inject("BASE_API_URL") public baseUrl: string,
    ) { }
    ngOnInit(): void {
      this.initColumns();
      this.getData();
    }
  /********************************* Initialize Data and Column ******************************************** */
  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._service.get(this.pageIndex, this.pageSize, this.searchValue,this.orderBy,this.ascending));
      this.data = result.data;
      this.totalPages = result.totalPages;
      this.totalRecords = result.totalRecords;
      this.getRequest = RequestStatus.Success;
    } catch (error) {
      console.log(error);
      this.getRequest = RequestStatus.Failed;

    }
  }
  initColumns() {
    this.columns = [
      {
        prop: "id",
        title: "#",
        show: true,
        sortable: true
      },
      {
        prop: "image",
        title: "Image",
        show: true,
        sortable: false
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
        sortable: false
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
      },
      {
        prop: "Actions",
        title: "Actions",
        show: true,
        sortable: false
      }
    ]
  }
  /********************************* Event Binding ******************************************** */

  onPageChange(event:PageSpec){
    this.pageIndex = event.pageIndex!;
    this.pageSize = event.pageSize!;
    this.getData();
  }
  onSortChange(event:SortSpec){
    this.orderBy = event.prop!;
    this.ascending = event.ascending;
    this.getData();
  }
  onSearch(value:string){
    this.searchValue = value;
    this.getData();
  }
  onCreate(){
    this.openForm();
  }
  onUpdate(item:Admin)
  {
    this.openForm(item);
  }
  onDeleteClick(id:number){
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if(res == true)
        this.delete(id);
      }
    })
  }
  /********************************* Form Configuration ******************************************** */

  getForm(item?: Admin, roles: Role[] = []): FormBuilderGroup[] {
    var controlGroups: FormBuilderGroup[] = [
      {

        title: "General",
        controls: [
          {
            title: "id",
            name: "id",
            controlType: ControlTypes.Hidden,
            width: "0px",
            value: item ? item.id : undefined
          },
          {
            title: "username",
            name: "username",
            icon: "person",
            controlType: ControlTypes.TextInput,
            width: "50%",
            value: item ? item.username : undefined,
            validators: [
              Validators.required,
              Validators.minLength(8),
              Validators.maxLength(25),
            ]
          },
          {
            title: "phone",
            name: "phone",
            icon: "phone",
            controlType: ControlTypes.NumberInput,
            width: "50%",
            value: item ? item.phone : undefined,
            validators: [
              Validators.required,
              Validators.minLength(10),
              Validators.maxLength(12),

            ]

          },
          {
            title: "email",
            name: "email",
            icon: "mail",
            controlType: ControlTypes.TextInput,
            width: "100%",
            value: item ? item.email : undefined,
            validators: [
              Validators.required
            ]

          },
          {
            title: "password",
            name: "password",
            icon: "key",

            controlType: ControlTypes.PasswordInput,
            width: "50%",
            validators: item ? [] : [
              Validators.required
            ]

          },
          {
            title: "re-enter the password",
            name: "repassword",
            icon: "key",
            validators: item ? [] : [
              Validators.required
            ],

            controlType: ControlTypes.PasswordInput,
            width: "50%"
          },
          {
            title: "Role",
            name: "roleId",
            controlType: ControlTypes.Selection,
            data: roles,
            isObjectData: true,
            labelProp: "title",
            valueProp: "id",
            width: "100%",
            value: item ? item.roleId : undefined,
            validators: [
              Validators.required
            ]


          },
          {
            title: "image",
            name: "image",
            controlType: ControlTypes.LocalFilePicker,
            width: "100%",
            value: item ? item.image : undefined,

          },
        ]
      }
    ];
    return controlGroups;
  }
 
  async openForm(item?: Admin) {
    var roles = await this.getRoles();
    var form = this.getForm(item, roles);
    this._dialog.open<FormBuilderComponent, FormBuilderPropsSpec, any>(FormBuilderComponent, {
      data: {
        title: "Create New Admin",
        controlsGroups: form,
        onSubmit : (result) => {
          this._dialog.closeAll();
          var admin = result as Admin;
          admin.image = !result['image'] ? 'none' : result['image'][0]['path'];
          if (item) {
            this.update(admin);
          }
          else
          this.create(admin);
        },
        onCancel:() => {
          this._dialog.closeAll();

        }
      },
      hasBackdrop:false,
      panelClass: "form-builder-dialog",
    })
  }
 
  /********************************* Api Integration ******************************************** */
  async getRoles(): Promise<Role[]> {
    try {
      this.dimRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._rolesService.get());
      console.log(result);
      this.dimRequest = RequestStatus.Success;
      return result.data;
    } catch (error) {
      console.log(error);

      this.dimRequest = RequestStatus.Failed;
      return [];
    }
  }
  create = async (item: Admin) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.post(item));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Item Created Successfully",
          title: "Success"
        }
      }).afterClosed().subscribe(_ => this._dialog.closeAll())
      this.getData();
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
      console.log(error);
    }
  }
  update = async (item: Admin) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.put(item));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Item Update Successfully",
          title: "Success"
        }
      }).afterClosed().subscribe(_ => this._dialog.closeAll())
      this.getData();
    } catch (error) {
      // this.dimRequest = RequestStatus.Failed;
      console.log(error);
    }
  }
  delete = async (id:number) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.delete(id));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Item Deleted Successfully",
          title: "Success"
        }
      }).afterClosed().subscribe(_ => this._dialog.closeAll())
      this.getData();
    } catch (error) {
      console.log(error);
    }
  }

 

}
