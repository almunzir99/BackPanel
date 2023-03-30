import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { RequestStatus } from './core/models/request-status.enum';
import { AuthService } from './core/services/auth.service';
import { GeneralService } from './core/services/general.service';
import { RolesService } from './core/services/roles.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  requestStatus = RequestStatus.Initial;
  theme: 'light' | 'dark' = 'light';
  constructor(private _authService: AuthService,
    private _roleService: RolesService,
    private router: Router,
    private _generalService: GeneralService,
    @Inject('DIRECTION') public dir: string,
    private overlay: OverlayContainer) {
    this._generalService.$theme.subscribe(value => {
      if (value == 'light') {
        var element = this.overlay.getContainerElement();
        if (element.classList.contains('dark-mode-theme'))
          element.classList.remove('dark-mode-theme')
        element.classList.add('light-mode-theme');
      } 
      else {
        var element = this.overlay.getContainerElement();
        if (element.classList.contains('light-mode-theme'))
          element.classList.remove('light-mode-theme')
        element.classList.add('dark-mode-theme');
      }
      this.theme = value;
    });
    this.getData();
  }
  async getData() {
    try {
      this.requestStatus = RequestStatus.Loading;
      var result = await firstValueFrom(this._authService.getCurrentUser());
      this._authService.setCurrentUser(result.data);
      if (!result.data.isManager) {
        var role = await firstValueFrom(this._roleService.single(result.data.roleId!));
        this._authService.$role.next(role.data);
      }
      var notifications = await firstValueFrom(this._authService.getNotifications());
      this._authService.$notifications.next(notifications.data);
      this.requestStatus = RequestStatus.Success;
    } catch (error) {
      this.requestStatus = RequestStatus.Failed;
      this.router.navigate(['/', 'authentication']);
    }
  }
  title = 'app';
}

