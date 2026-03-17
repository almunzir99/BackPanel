import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BusinessRoutingModule } from './business-routing.module';
import { BusinessComponent } from './business.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';



@NgModule({
  declarations: [
    BusinessComponent
  ],
  imports: [
    CommonModule,
    BusinessRoutingModule,
    SharedModule,
    MatIconModule,
    MatDialogModule,
    MatTabsModule,
  ]
})
export class BusinessModule { }
