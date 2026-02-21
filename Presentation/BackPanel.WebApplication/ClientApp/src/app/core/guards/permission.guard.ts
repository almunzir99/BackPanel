import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MenuList } from 'src/app/dashboard/components/menu/menu.list';
import { Admin } from '../models/admin.model';
import { Role } from '../models/role.model';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard  {
  currentRole: Role | null = null;
  currentUser: Admin | null = null;
  menuList = MenuList;
  constructor(_authService: AccountService,private router:Router) {
    _authService.$role.subscribe(role => {
      this.currentRole = role
    })
    _authService.$currentUser.subscribe(user => this.currentUser = user)

  }
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if (this.currentUser?.isManager) return true;
    var permitted = this.checkIfRouteAccessPermitted(state.url);
    if(!permitted)
    this.router.navigate(['/','dashboard','home']);
    return permitted;

  }
  checkIfRouteAccessPermitted(route: string): boolean {
    var result = false;
    const currentRoleTitle = this.currentRole?.title?.toLowerCase().trim();
    this.menuList.forEach(group => {
      group.children!.forEach(item => {
        if (item.route == route) {
          if (!item.allowedRoles || item.allowedRoles.length == 0) {
            result = true;
            return;
          }
          if (currentRoleTitle) {
            result = item.allowedRoles.some(x => x.toLowerCase().trim() == currentRoleTitle);
            return;
          }
        }
         
      });
    });
    return result;
  }


}
