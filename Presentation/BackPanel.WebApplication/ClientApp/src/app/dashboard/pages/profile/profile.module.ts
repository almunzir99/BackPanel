import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTabsModule } from "@angular/material/tabs";

@NgModule({
  declarations: [
    ProfileComponent
  ],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    SharedModule,
    MatIconModule,
    MatDialogModule,
    MatTabsModule
  ]
})
export class ProfileModule { }
