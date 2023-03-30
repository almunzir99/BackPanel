import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { Activity } from 'src/app/core/models/activity.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { PagedResponse } from 'src/app/core/models/wrappers/paged-response.model';
import { AdminsService } from 'src/app/core/services/admins.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { Column } from 'src/app/shared/components/datatable/column.model';
import { PageSpec } from 'src/app/shared/components/datatable/datatable.component';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
})
export class ActivitiesComponent implements OnInit {
  columns: Column[] = [];
  data: Activity[] = [];
  pageIndex = 1;
  pageSize = 10;
  totalRecords = 0;
  totalPages = 1;
  getRequest = RequestStatus.Initial;
  id:number | null = null;
  theme:'light' | 'dark' = 'light';
  constructor(private _service:AdminsService,private route:ActivatedRoute,_generalService:GeneralService) { 
    this.id = this.route.snapshot.params['id'];
    _generalService.$theme.subscribe(value => this.theme = value);

  }
  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var result :PagedResponse<Activity[]> | null = null;
      if(this.id)
        result = await firstValueFrom(this._service.getAdminActivities(this.id!,this.pageIndex, this.pageSize));
      else
      result = await firstValueFrom(this._service.getActivities(this.pageIndex, this.pageSize));
      this.data = result!.data;
      this.totalPages = result!.totalPages;
      this.totalRecords = result!.totalRecords;
      console.log(this.totalRecords);
      this.getRequest = RequestStatus.Success;
    } catch (error) {
      this.getRequest = RequestStatus.Failed;
    }
  }
  onPageChange(event:PageSpec){
    this.pageIndex = event.pageIndex!;
    this.pageSize = event.pageSize!;
    this.getData();
  }
  initCols() {
    this.columns = [
      {
        title: "Actor",
        prop: "admin",
        sortable: false,
        show: true
      },
      {
        title: "Effected Table",
        prop: "effectedTable",
        sortable: false,
        show: true
      },
      {
        title: "Effected Row Id",
        prop: "effectedRowId",
        sortable: false,
        show: true
      },
      {
        title: "Happened At",
        prop: "createdAt",
        sortable: false,
        show: true
      },

      {
        title: "Action",
        prop: "action",
        sortable: false,
        show: true
      }
    ]
  }

  ngOnInit(): void {
    this.initCols();
    this.getData();
    
  }

}
