import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MenuComponent } from './components/menu/menu.component';
import { HeaderComponent } from './components/header/header.component';
import { DashboardComponent } from './dashboard.component';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { CoreModule } from '../core/core.module';



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
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    CoreModule
  ]
})
export class DashboardModule { }
