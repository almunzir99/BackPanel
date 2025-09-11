import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MenuComponent } from './components/menu/menu.component';
import { HeaderComponent } from './components/header/header.component';
import { DashboardComponent } from './dashboard.component';
import { MatRippleModule } from '@angular/material/core';
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatLegacyMenuModule as MatMenuModule } from '@angular/material/legacy-menu';
import { MatIconModule } from '@angular/material/icon';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';



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
    CoreModule,
    SharedModule
  ]
})
export class DashboardModule { }
