import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MenuComponent } from './components/menu/menu.component';
import { HeaderComponent } from './components/header/header.component';
import { DashboardComponent } from './dashboard.component';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [
    MenuComponent,
    HeaderComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule, 
    MatRippleModule,
    MatButtonModule
  ]
})
export class DashboardModule { }
