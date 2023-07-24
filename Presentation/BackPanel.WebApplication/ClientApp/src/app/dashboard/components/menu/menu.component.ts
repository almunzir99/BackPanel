import { Component, Inject, OnInit } from '@angular/core';
import { Admin } from 'src/app/core/models/admin.model';
import { Permission } from 'src/app/core/models/permission.model';
import { Role } from 'src/app/core/models/role.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { MenuGroup } from './menu.group';
import { MenuList } from './menu.list';
import { CompanyInfo } from 'src/app/core/models/company-info.model';
import { CompanyInfoService } from 'src/app/core/services/company-info.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'dahboard-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  menuList = JSON.parse(JSON.stringify(MenuList)) as MenuGroup[];
  currentUser: Admin | null = null;
  currentRole: Role | null = null;
  theme: 'light' | 'dark' = 'light';
  dir:'rtl' | 'ltr' = 'rtl';
  company:CompanyInfo | null;
  constructor(_authService: AuthService,
    _generalService: GeneralService,
    _companyInfoService:CompanyInfoService,
    @Inject("BASE_API_URL") public baseUrl: string,
     _translateService:TranslateService) {
    _authService.$currentUser.subscribe(res => {
      this.currentUser = res;
    })
    this.company = _companyInfoService.$companyIfo;
    _translateService.onLangChange.subscribe({
      next: (value:any) => {
          this.dir = value.lang == 'ar' ? 'rtl' : 'ltr';
      }
    })
    _generalService.$theme.subscribe(value => this.theme = value);
    _authService.$role.subscribe(res => {
      this.currentRole = res;
      this.displayOnlyPermittedItems();
    });
  }
  displayOnlyPermittedItems() {
    this.menuList = JSON.parse(JSON.stringify(MenuList)) as MenuGroup[];
    this.menuList.forEach(group => {
      group.children = group.children?.filter(c => {
        return this.checkIfMenuItemPermittedToDisplay(c.permissionName!);
      });
    });
    console.log(this.menuList)
  }
  checkIfMenuItemPermittedToDisplay(permissionName: string): boolean {
    if (this.currentUser?.isManager)
      return true;
    else if (permissionName == "generalPermissions")
      return true;
    else if (this.currentRole) {
      var permission = (this.currentRole as any)[permissionName] as Permission;
      if (permission)
        return permission.read;
      return false;
    }
    return false;
  }
  ngOnInit(): void {
  }

}
