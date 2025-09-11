import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TranslationEditorRoutingModule } from './translation-editor-routing.module';
import { TranslationEditorComponent } from './translation-editor.component';
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatRippleModule } from '@angular/material/core';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatLegacyFormFieldModule as MatFormFieldModule } from '@angular/material/legacy-form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatLegacyListModule as MatListModule} from '@angular/material/legacy-list';
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
