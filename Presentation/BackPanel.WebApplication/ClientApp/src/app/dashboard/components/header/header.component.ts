import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ApiNotification } from 'src/app/core/models/api-notification.model';
import { AccountService } from 'src/app/core/services/account.service';
import * as dayjs from 'dayjs';
import * as relativeTime from 'dayjs/plugin/relativeTime';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { firstValueFrom } from 'rxjs';
import { GeneralService } from 'src/app/core/services/general.service';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { NotificationsService } from 'src/app/core/services/notifications.service';
dayjs.extend(relativeTime)
@Component({
  selector: 'dashboard-header',
  templateUrl: './header.component.html',
  standalone: false,
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Output("toggleClick") toggleClickEventEmitter = new EventEmitter<boolean>();
  @Output("languageChange") languageChange = new EventEmitter<string>();
  @Input("toggle") toggle = false;
  opened = false;
  isFullScreen = false;
  unread = 0;
  notifications: ApiNotification[] = [];
  readRequest = RequestStatus.Initial;
  theme: 'light' | 'dark' = 'light';
  currentLang = "en";
  constructor(private _authService: AccountService, private router: Router,
    private _generalService: GeneralService,
    private _translateService: TranslateService,
    private _notificationsService: NotificationsService
  ) {
    _generalService.$theme.subscribe(value => this.theme = value);
    this.currentLang = _translateService.currentLang;
  }

  ngOnInit(): void {
    this._notificationsService.$notifications.subscribe(res => {
      this.notifications = res;
      this.unread = res.filter(c => c.read == false).length;
    })
  }
  toggleTheme() {
    if (this.theme == 'dark')
      this._generalService.$theme.next('light');
    else
      this._generalService.$theme.next('dark')

  }
  onToggle() {
    this.toggle = !this.toggle;
    this.toggleClickEventEmitter.emit(this.toggle);
  }
  logout() {
    this._authService.logout();
    this.router.navigate(['authentication']);
  }
  onReadNotificationClick() {
    this.readNotification();
  }
  openFullScreen() {
    var body = document.getElementById("main-body");
    if (body) {
      if (!this.isFullScreen) {
        body.requestFullscreen();
        this.isFullScreen = true;
      }
      else {
        document.exitFullscreen();
        this.isFullScreen = false;
      }
    }
  }
  async readNotification() {
    try {
      this.readRequest = RequestStatus.Loading;
      await firstValueFrom(this._notificationsService.readNotifications());
      this._notificationsService.$notifications.value.forEach(n => n.read = true);
      this.unread = 0;
      this.readRequest = RequestStatus.Success;


    } catch (error) {
      this.readRequest = RequestStatus.Failed;
    }
  }
  formatDate(date: string): string {
    return dayjs(date).fromNow()
  }

}
