import { Component, Inject, OnInit } from '@angular/core';
import { Admin } from 'src/app/core/models/admin.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { MenuList } from './menu.list';

@Component({
  selector: 'dahboard-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  menuList = MenuList;
  currentUser:Admin | null = null;
  constructor(private _authService:AuthService,@Inject("BASE_API_URL") public baseUrl: string,@Inject('DIRECTION') public dir:string) {
    this.currentUser = _authService.$currentUser.value;
   }

  ngOnInit(): void {
  }

}
