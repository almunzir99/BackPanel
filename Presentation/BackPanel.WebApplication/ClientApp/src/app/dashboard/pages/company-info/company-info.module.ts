import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyInfoRoutingModule } from './company-info-routing.module';
import { CompanyInfoComponent } from './company-info.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatLegacyTabsModule as MatTabsModule } from '@angular/material/legacy-tabs';


@NgModule({
  declarations: [
    CompanyInfoComponent
  ],
  imports: [
    CommonModule,
    CompanyInfoRoutingModule,
    SharedModule,
    MatIconModule,
    MatDialogModule,
    MatTabsModule,
  ]
})
export class CompanyInfoModule { }
