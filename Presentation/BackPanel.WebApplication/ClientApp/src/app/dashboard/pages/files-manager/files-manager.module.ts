import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesManagerRoutingModule } from './files-manager-routing.module';
import { FilesManagerComponent } from './files-manager.component';


@NgModule({
  declarations: [
    FilesManagerComponent
  ],
  imports: [
    CommonModule,
    FilesManagerRoutingModule
  ]
})
export class FilesManagerModule { }
