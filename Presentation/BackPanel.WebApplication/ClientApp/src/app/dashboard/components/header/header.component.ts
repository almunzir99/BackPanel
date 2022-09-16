import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ApiNotification } from 'src/app/core/models/api-notification.model';
import { AuthService } from 'src/app/core/services/auth.service';
import * as dayjs from 'dayjs';
import * as relativeTime from 'dayjs/plugin/relativeTime';
import { RequestStatus } from 'src/app/core/models/request-status.enum';
import { firstValueFrom } from 'rxjs';
dayjs.extend(relativeTime)
@Component({
  selector: 'dashboard-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Output("toggleClick") toggleClickEventEmitter = new EventEmitter<boolean>();
  @Input("toggle") toggle = false;
  opened = false;
  isFullScreen = false;
  unread = 0;
  notifications: ApiNotification[] = [];
  readRequest = RequestStatus.Initial;
  constructor(private _authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this._authService.$notifications.subscribe(res => {
      this.notifications = res;
      this.unread = res.filter(c => c.read == false).length;
    })
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
      await firstValueFrom(this._authService.readNotifications());
      this._authService.$notifications.value.forEach(n => n.read = true);
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
