import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivitiesComponent } from './activities/activities.component';
import { AdminsComponent } from './admins.component';

const routes: Routes = [
  {
    path:'',
    component:AdminsComponent
  },
  {
    path:'activities',
    loadChildren:() => import('./activities/activities.module').then(c => c.ActivitiesModule)
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminsRoutingModule { }
