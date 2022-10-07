import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MenuList } from 'src/app/dashboard/components/menu/menu.list';
import { Admin } from '../models/admin.model';
import { Permission } from '../models/permission.model';
import { Role } from '../models/role.model';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivateChild {
  currentRole: Role | null = null;
  currentUser: Admin | null = null;
  menuList = MenuList;
  constructor(_authService: AuthService,private router:Router) {
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
    this.menuList.forEach(group => {
      group.children!.forEach(item => {
        if (item.route == route) {
          if (item.permissionName == "generalPermissions") {
            console.log(item.permissionName)
            result = true;
            return;
          }
          else {
            if(this.currentRole)
            {
              var permission = (this.currentRole as any)[item.permissionName!] as Permission;
              if (permission)
                result = permission.read;
              return;
            }
          }
        }
         
      });
    });
    console.log(result)
    return result;
  }


}
