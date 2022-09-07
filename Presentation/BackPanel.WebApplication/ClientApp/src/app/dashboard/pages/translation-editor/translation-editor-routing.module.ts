import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslationEditorComponent } from './translation-editor.component';

const routes: Routes = [
  {
    path:'',
    component:TranslationEditorComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TranslationEditorRoutingModule { }
