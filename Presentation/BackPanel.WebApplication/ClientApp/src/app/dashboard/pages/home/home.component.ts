import { Component, Inject, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Message } from 'src/app/core/models/message.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { Stats } from 'src/app/core/models/stats.model';
import { MessagesService } from 'src/app/core/services/messages.service';
import { StatisticsService } from 'src/app/core/services/statistics.service';
import { Column } from 'src/app/shared/components/datatable/column.model';
import * as dayjs from 'dayjs';
import { Activity } from 'src/app/core/models/activity.model';
import { AdminsService } from 'src/app/core/services/admins.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  stats: Stats | null = null;
  getRequest = RequestStatus.Initial;
  counterCards: CounterCardSpec[] = [];
  messageRequest = RequestStatus.Loading;
  activitiesRequest = RequestStatus.Loading;
  activitiesCols: Column[] = [];
  messages: Message[] = [];
  activities: Activity[] = [];
  constructor(private _service: StatisticsService, private _adminService:AdminsService, private _messageSerivce: MessagesService,@Inject('DIRECTION') public dir:string) {

  }
  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._service.getStats());
      this.stats = result.data;
      this.initCards();
      this.getRequest = RequestStatus.Success;
      await this.getMessages();
      await this.getActivities();
    } catch (error) {
      this.getRequest = RequestStatus.Failed;
    }
  }
  async getMessages() {
    try {
      this.messageRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._messageSerivce.get());
      this.messages = result.data;
      this.messageRequest = RequestStatus.Success;
    } catch (error) {
      this.messageRequest = RequestStatus.Failed;
    }
  }
  async getActivities() {
    try {
      this.activitiesRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._adminService.getActivities());
      this.activities = result.data;
      this.activitiesRequest = RequestStatus.Success;
    } catch (error) {
      this.activitiesRequest = RequestStatus.Failed;
    }
  }
  ngOnInit(): void {
    this.initActivitiesCols();
    this.getData();
  }
  initActivitiesCols() {
    this.activitiesCols = [
      {
        title: "Actor",
        prop: "admin",
        sortable: true,
        show: true
      },
      {
        title: "Effected Table",
        prop: "effectedTable",
        sortable: true,
        show: true
      },
      {
        title: "Effected Row Id",
        prop: "effectedRowId",
        sortable: true,
        show: true
      },
      {
        title: "Action",
        prop: "action",
        sortable: true,
        show: true
      }
    ]
  }
  initCards() {
    this.counterCards = [
      {
        title: "Admins",
        count: this.stats?.admins!,
        icon: "las la-user-tie",
        color: "#4a4cfb"
      },
      {
        title: "Roles",
        count: this.stats?.roles!,
        icon: "las la-user-cog",
        color: "#ff9e20"
      }
      ,
      {
        title: "Messages",
        count: this.stats?.messages!,
        icon: "las la-envelope",
        color: "#4a4cfb"
      }
    ];
  }
  formatDate(date: string): string {
    return dayjs(date).format('MMM DD')
  }
}

export interface CounterCardSpec {
  title: string;
  count: number;
  color: string;
  icon: string;
}