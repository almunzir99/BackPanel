import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Permission } from 'src/app/core/models/permission.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { Role } from 'src/app/core/models/role.model';
import { GeneralService } from 'src/app/core/services/general.service';
import { RolesService } from 'src/app/core/services/roles.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { PageSpec, SortSpec } from 'src/app/shared/components/datatable/datatable.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  columns: Column[] = [];
  data: Role[] = [];
  pageIndex = 1;
  pageSize = 10;
  totalRecords = 0;
  totalPages = 1;
  orderBy = "lastUpdate";
  ascending = false;
  searchValue = "";
  getRequest = RequestStatus.Initial;
  dimRequest = RequestStatus.Initial;
  templateOnlyShow:boolean = false; // wether to show only permissions table or  from
  theme:'light' | 'dark' = 'light';
  @ViewChild("roleForm") roleForm?: TemplateRef<any>;
  role: any;
  constructor(private _service: RolesService, private _dialog: MatDialog,_generalService:GeneralService) { 
    _generalService.$theme.subscribe(value => this.theme = value);

  }
  ngOnInit(): void {
    this.initRole();
    this.initColumns();
    this.getData();
  }
  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._service.get(this.pageIndex, this.pageSize, this.searchValue, this.orderBy, this.ascending));
      this.data = result.data;
      this.totalPages = result.totalPages;
      this.totalRecords = result.totalRecords;
      this.getRequest = RequestStatus.Success;
    } catch (error) {
      this.getRequest = RequestStatus.Failed;
    }
  }
  initRole(){
    this.role = {
      id: 0,
      title: '',
      rolesPermissions: { create: false, read: false, update: false, delete: false },
      adminsPermissions: { create: false, read: false, update: false, delete: false },
      messagesPermissions: { create: false, read: false, update: false, delete: false }
    };
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
        prop: "title",
        title: "Title",
        show: true,
        sortable: true
      },
      {
        prop: "permissions",
        title: "Permissions",
        show: true,
        sortable: false
      },
      {
        prop: "createdAt",
        title: "CreatedAt",
        show: true,
        sortable: true
      },
      {
        prop: "lastUpdate",
        title: "LastUpdate",
        show: true,
        sortable: true
      },
      {
        prop: "Actions",
        title: "Actions",
        show: true,
        sortable: false
      }
    ];
  }
  getRoleKeys(): string[] {
    return Object.keys(this.role).filter(c => c.includes("Permissions")).map(o => o.replace("Permissions", ""));
  }

  /********************************* Event Binding ******************************************** */

  onPageChange(event: PageSpec) {
    this.pageIndex = event.pageIndex!;
    this.pageSize = event.pageSize!;
    this.getData();
  }
  onSortChange(event: SortSpec) {
    this.orderBy = event.prop!;
    this.ascending = event.ascending;
    this.getData();
  }
  onSearch(value: string) {
    this.searchValue = value;
    this.getData();
  }
  onCreate() {
    this.openForm();
  }
  onUpdate(item: Role) {
    this.openForm(item);
  }
  onDeleteClick(id: number) {
    this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
      data: {
        type: MessageTypes.CONFIRM,
        message: "Are Sure you want to Delete this Item ?",
        title: "Confirm"
      }
    }).afterClosed().subscribe({
      next: (res) => {
        if (res == true)
          this.delete(id);
      }
    })
  }
  onFormSubmit() {
    this._dialog.closeAll();
    if (this.role['id'] == 0)
      this.create(this.role);
    else {
      this.update(this.role); 
    }
  }
  onShowPermissionsClick(item:Role){
    this.role = item;
    this.templateOnlyShow = true;
    this._dialog.open(this.roleForm!);
  }
  closeDialog = () => this._dialog.closeAll();
  onExportClick(type: string) {
    this.dimRequest = RequestStatus.Loading;
    this._service.export(type,() => {
      this.dimRequest = RequestStatus.Success;
    },(err) => {
      this.dimRequest = RequestStatus.Failed;
    })
  }
  /********************************* Form Configuration ******************************************** */

  openForm(item?: Role) {
    this.templateOnlyShow = false;
    if(item)
    this.role = item;
    else
    this.initRole();
    this._dialog.open(this.roleForm!);
  }
  /********************************* Api Integration ******************************************** */

  create = async (item: Role) => {
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
    }
  }
  update = async (item: Role) => {
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
      this.dimRequest = RequestStatus.Failed;
    }
  }
  delete = async (id: number) => {
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
      this.dimRequest = RequestStatus.Failed;
    }
  }


}

