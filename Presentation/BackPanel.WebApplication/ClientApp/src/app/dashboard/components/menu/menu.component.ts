import { Component, OnInit } from '@angular/core';
import { MenuList } from './menu.list';

@Component({
  selector: 'dahboard-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  menuList = MenuList;
  constructor() { }

  ngOnInit(): void {
  }

}
