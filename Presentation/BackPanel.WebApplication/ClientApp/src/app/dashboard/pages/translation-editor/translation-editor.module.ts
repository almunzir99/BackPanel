import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TranslationEditorRoutingModule } from './translation-editor-routing.module';
import { TranslationEditorComponent } from './translation-editor.component';


@NgModule({
  declarations: [
    TranslationEditorComponent
  ],
  imports: [
    CommonModule,
    TranslationEditorRoutingModule
  ]
})
export class TranslationEditorModule { }
