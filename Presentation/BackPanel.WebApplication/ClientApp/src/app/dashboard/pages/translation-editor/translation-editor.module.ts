import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TranslationEditorRoutingModule } from './translation-editor-routing.module';
import { TranslationEditorComponent } from './translation-editor.component';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatListModule} from '@angular/material/list';
import { QuillModule } from 'ngx-quill';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [
    TranslationEditorComponent
  ],
  imports: [
    CommonModule,
    TranslationEditorRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatRippleModule,
    MatButtonModule,
    MatDialogModule,
    MatExpansionModule,
    MatListModule,
    FormsModule,
    SharedModule,
    MatDialogModule,
    QuillModule.forRoot(),

  ]
})
export class TranslationEditorModule { }
