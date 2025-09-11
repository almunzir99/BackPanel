import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesManagerRoutingModule } from './files-manager-routing.module';
import { FilesManagerComponent } from './files-manager.component';
import { MatLegacyFormFieldModule as MatFormFieldModule } from '@angular/material/legacy-form-field';
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input';
import { MatIconModule } from '@angular/material/icon';
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatRippleModule } from '@angular/material/core';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MatLegacyMenuModule as MatMenuModule } from '@angular/material/legacy-menu';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatLegacySnackBarModule as MatSnackBarModule } from '@angular/material/legacy-snack-bar';
import { SharedModule } from 'src/app/shared/shared.module';
import { CoreModule } from 'src/app/core/core.module';
import { MatLegacyCheckboxModule as MatCheckboxModule } from '@angular/material/legacy-checkbox';

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
