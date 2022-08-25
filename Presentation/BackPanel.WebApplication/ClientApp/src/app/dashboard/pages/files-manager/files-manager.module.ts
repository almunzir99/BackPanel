import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesManagerRoutingModule } from './files-manager-routing.module';
import { FilesManagerComponent } from './files-manager.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SharedModule } from 'src/app/shared/shared.module';
import { CoreModule } from 'src/app/core/core.module';
import { MatCheckboxModule } from '@angular/material/checkbox';

@NgModule({
  declarations: [
    FilesManagerComponent
  ],
  imports: [
    CommonModule,
    FilesManagerRoutingModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatIconModule,
    MatRippleModule,
    MatButtonModule,
    MatMenuModule,
    MatDialogModule,
    MatSnackBarModule,
    SharedModule,
    CoreModule,
    AngularSvgIconModule.forRoot(),

  ]
})
export class FilesManagerModule { }
