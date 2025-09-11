import { Component, Inject, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Message } from 'src/app/core/models/message.model';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { counters } from 'src/app/core/models/counters.model';
import { MessagesService } from 'src/app/core/services/messages.service';
import { DashboardService } from 'src/app/core/services/dashboard.service';
import * as dayjs from 'dayjs';
import { AdminsService } from 'src/app/core/services/admins.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { Role } from 'src/app/core/models/role.model';
import { AccountService } from 'src/app/core/services/account.service';
import { Admin } from 'src/app/core/models/admin.model';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  standalone: false,
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  stats: counters | null = null;
  getRequest = RequestStatus.Initial;
  counterCards: CounterCardSpec[] = [];
  messageRequest = RequestStatus.Loading;
  messages: Message[] = [];
  theme: 'light' | 'dark' = 'light';
  currentRole: Role | null = null;
  currentUser: Admin | null = null;
  constructor(
    private _service: DashboardService,
    private _adminService: AdminsService,
    private _messageSerivce: MessagesService,
    _authService: AccountService,
    @Inject('DIRECTION') public dir: string,
    _generalService: GeneralService, private _translateService: TranslateService) {
    this.dir = _translateService.currentLang == 'ar' ? 'rtl' : 'ltr';
    _generalService.$theme.subscribe(value => this.theme = value);
    _authService.$role.subscribe(res => this.currentRole = res);
    _authService.$currentUser.subscribe(res => this.currentUser = res);

  }
  async getData() {
    try {
      this.getRequest = RequestStatus.Loading;
      var result = await firstValueFrom(this._service.getCounters());
      this.stats = result.data;
      this.initCards();
      this.getRequest = RequestStatus.Success;
      if (this.currentRole?.messagesPermissions.read || this.currentUser?.isManager)
        await this.getMessages();

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
  ngOnInit(): void {
    this.getData();
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