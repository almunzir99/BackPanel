import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminsRoutingModule } from './admins-routing.module';
import { AdminsComponent } from './admins.component';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';


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
