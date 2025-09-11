import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminsRoutingModule } from './admins-routing.module';
import { AdminsComponent } from './admins.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [
    AdminsComponent
  ],
  imports: [
    CommonModule,
    AdminsRoutingModule,
    MatDialogModule,
    SharedModule,
    MatButtonModule,
    MatRippleModule,
    MatIconModule
  ]
})
export class AdminsModule { }
