import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { RequestStatus } from './core/models/request-status.enum';
import { AuthService } from './core/services/auth.service';
import { GeneralService } from './core/services/general.service';
import { RolesService } from './core/services/roles.service';
import { CompanyInfoService } from './core/services/company-info.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from './shared/constants/app_constants';

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
    private _companyInfoService: CompanyInfoService,
    @Inject('DIRECTION') public dir: string,
    private _translateService: TranslateService,
    private overlay: OverlayContainer) {
    // Configure Translations
    _translateService.addLangs(["en","ar"]);
    _translateService.setDefaultLang("en");
    // Configure themes
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
  ngAfterContentInit(){
    this.loadLang();
    this._translateService.onLangChange.subscribe({ 
      next: (value:any) => {
          this.dir = value.lang == 'ar' ? 'rtl' : 'ltr';
      }
    })
  }
  async getData() {
    try {
      this.requestStatus = RequestStatus.Loading;
      var result = await firstValueFrom(this._authService.getCurrentUser());
      var companyInfo = await firstValueFrom(this._companyInfoService.single());
      this._authService.setCurrentUser(result.data);
      if (!result.data.isManager) {
        var role = await firstValueFrom(this._roleService.single(result.data.roleId!));
        this._authService.$role.next(role.data);
      }
      // set company info
      this._companyInfoService.setCompanyInfo(companyInfo.data);
      var notifications = await firstValueFrom(this._authService.getNotifications());
      this._authService.$notifications.next(notifications.data);
      this.requestStatus = RequestStatus.Success;
    } catch (error) {
      this.requestStatus = RequestStatus.Failed;
      this.router.navigate(['/', 'authentication']);
    }
  }
  loadLang() {
    var lang = localStorage.getItem(AppConstants.LanguageLocalStorageKey);
    this._translateService.use(lang ?? "en");
  }
  title = 'app';
}

