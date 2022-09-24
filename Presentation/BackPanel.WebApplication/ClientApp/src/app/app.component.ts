import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { RequestStatus } from './core/models/request-status.enum';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  requestStatus = RequestStatus.Initial;
  constructor(private _authService:AuthService,private router:Router,@Inject('DIRECTION') public dir:string) {
    this.getCurrentUser();
  }
  async getCurrentUser() {
    try {
      this.requestStatus = RequestStatus.Loading;
      var result = await firstValueFrom(this._authService.getCurrentUser());
      this._authService.setCurrentUser(result.data);
      var notifications = await firstValueFrom(this._authService.getNotifications());
      this._authService.$notifications.next(notifications.data);
      console.log(notifications);
      this.requestStatus = RequestStatus.Success;
    } catch (error) {
      console.log(error);
      this.requestStatus = RequestStatus.Failed;
      this.router.navigate(['/','authentication']);
    }
    


  }
  title = 'app';
}

