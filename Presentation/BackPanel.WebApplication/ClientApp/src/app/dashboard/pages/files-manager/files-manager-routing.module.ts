import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FilesManagerComponent } from './files-manager.component';

const routes: Routes = [
  {
    path:'',
    component:FilesManagerComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FilesManagerRoutingModule { }
