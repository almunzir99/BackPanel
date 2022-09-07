import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';

const routes: Routes = [
 {
  path:'',
  component:DashboardComponent,
  children: [
    {
      path:'',
      redirectTo:'home',
      pathMatch:'full'
    },
    {
      path: 'home',
      loadChildren: () => import('./pages/home/home.module').then(c => c.HomeModule)
    },
    {
      path: 'admins',
      loadChildren: () => import('./pages/files-manager/admins/admins.module').then(c => c.AdminsModule)
    },
    {
      path: 'roles',
      loadChildren: () => import('./pages/roles/roles.module').then(c => c.RolesModule)
    },
    {
      path: 'messages',
      loadChildren: () => import('./pages/messages/messages.module').then(c => c.MessagesModule)
    },
    {
      path: 'translation-editor',
      loadChildren: () => import('./pages/translation-editor/translation-editor.module').then(c => c.TranslationEditorModule)
    },
    {
      path: 'profile',
      loadChildren: () => import('./pages/profile/profile.module').then(c => c.ProfileModule)
    },
    {
      path: 'settings',
      loadChildren: () => import('./pages/settings/settings.module').then(c => c.SettingsModule)
    },
    {
      path: 'files-manager',
      loadChildren: () => import('./pages/files-manager/files-manager.module').then(c => c.FilesManagerModule)
    },
  ]
 }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
