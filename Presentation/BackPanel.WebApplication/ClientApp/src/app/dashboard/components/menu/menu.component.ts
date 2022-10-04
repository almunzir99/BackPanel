import { Component, Inject, OnInit } from '@angular/core';
import { Admin } from 'src/app/core/models/admin.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { MenuList } from './menu.list';

@Component({
  selector: 'dahboard-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  menuList = MenuList;
  currentUser:Admin | null = null;
  theme:'light' | 'dark' = 'light';
  constructor(_authService:AuthService, _generalService:GeneralService,@Inject("BASE_API_URL") public baseUrl: string,@Inject('DIRECTION') public dir:string) {
    this.currentUser = _authService.$currentUser.value;
    _generalService.$theme.subscribe(value => this.theme = value);

   }

  ngOnInit(): void {
  }

}
