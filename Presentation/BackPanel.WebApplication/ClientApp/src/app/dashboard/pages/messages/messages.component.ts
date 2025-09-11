import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Message } from 'src/app/core/models/message.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { MessagesService } from 'src/app/core/services/messages.service';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { PageSpec, SortSpec } from 'src/app/shared/components/datatable/datatable.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  columns: Column[] = [];
  data: Message[] = [];
  pageIndex = 1;
  pageSize = 10;
  totalRecords = 0;
  totalPages = 1;
  orderBy = "lastUpdate";
  ascending = false;
  searchValue = "";
  getRequest = RequestStatus.Initial;
  dimRequest = RequestStatus.Initial;
  selectedShowMessage: Message | null = null;
  @ViewChild("showMessageDialog") showMessageDialog?: TemplateRef<any>;
  constructor(
    private _service: MessagesService,
    private _dialog: MatDialog,
  ) { }
  ngOnInit(): void {
    this.initColumns();
    this.getData();
  }

  /********************************* Initialize Data and Column ******************************************** */
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
  initColumns() {
    this.columns = [
      {
        title: "Id",
        prop: "id",
        show: true,
        sortable: true
      },
      {
        title: "full Name",
        prop: "fullName",
        show: true,
        sortable: true,
        importable:true
      },
      {
        title: "email",
        prop: "email",
        show: true,
        sortable: true,
        importable:true

      },
      {
        title: "phone",
        prop: "phone",
        show: true,
        sortable: true,
        importable:true

      },
      {
        title: "Content",
        prop: "content",
        show: true,
        sortable: true,
        importable:true

      },
      {
        title: "Sent At",
        prop: "createdAt",
        show: true,
        sortable: true,

      },
      {
        title: "Actions",
        prop: "actions",
        show: true,
        sortable: false,
      }
    ]
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
  onShowMessageClick(message: Message) {
    this.selectedShowMessage = message;
    this._dialog.open(this.showMessageDialog!);
  }
  closeDialog() {
    this._dialog.closeAll();
  }
  onPrint(message: Message) {
    var a = window.open('', '', 'height=800, width=800');
    if (a) {
      a.document.write('<html>');
      a.document.write('<body > ');
      var item = ` <div  style="padding: 30px;font-family: 'Poppin';">
    <h3> Message Id:  ${message.id}</h3>
    <h3> From :  ${message.fullName}</h3>
    <h3>Sender Email :  ${message.fullName}</h3>
    <h3> Sender Phone :  ${message.phone}</h3>

    <br>
    <p>${message.content}</p>
</div>`;
      a.document.write(item);
      a.document.write('</body></html>');
      a.document.close();
      a.print();
    }
  }
    onExportClick(type: string) {
    this.dimRequest = RequestStatus.Loading;
    this._service.export(type,() => {
      this.dimRequest = RequestStatus.Success;
    },(err) => {
      this.dimRequest = RequestStatus.Failed;
    })
  }
  onImportData(data:any[]) {
    this.createAll(data);
  }

  /********************************* Api Integration ******************************************** */
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
  createAll = async (items: any[]) => {
    try {
      this.dimRequest = RequestStatus.Loading;
      await firstValueFrom(this._service.postAll(items));
      this.dimRequest = RequestStatus.Success;
      this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
        data: {
          type: MessageTypes.SUCCESS,
          message: "Items Added Successfully",
          title: "Success"
        }
      }).afterClosed().subscribe(_ => this._dialog.closeAll())
      this.getData();
    } catch (error) {
      this.dimRequest = RequestStatus.Failed;
    }
  }

}
