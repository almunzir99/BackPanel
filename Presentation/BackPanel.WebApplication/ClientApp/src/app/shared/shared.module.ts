import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilderComponent } from './components/form-builder/form-builder.component';
import {ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
 
import {MatDatepickerModule} from '@angular/material/datepicker';
import { QuillModule } from 'ngx-quill';
import { DatatableComponent } from './components/datatable/datatable.component';
 
import { AngularSvgIconModule } from 'angular-svg-icon';
import { OfflineComponent } from './components/placeholders/offline/offline.component';
import { EmptyComponent } from './components/placeholders/empty/empty.component';
import { SpinnerComponent } from './components/placeholders/spinner/spinner.component';
import { DimmerLoadingComponent } from './components/dimmer-loading/dimmer-loading.component';
import { AlertMessageComponent } from './components/alert-message/alert-message.component';
import { LocalFilesPickerComponent } from './components/local-files-picker/local-files-picker.component';
import { MatDialogModule } from '@angular/material/dialog';

import { FieldsMatcherComponent } from './components/fields-matcher/fields-matcher.component';
import {DragDropModule} from '@angular/cdk/drag-drop';
import { LanguagesSwitchComponent } from './components/languages-switch/languages-switch.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import {MatSelectModule} from '@angular/material/select';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatAutocompleteModule} from '@angular/material/autocomplete';

@NgModule({ 
  declarations: [
    FormBuilderComponent,
    DatatableComponent,
    OfflineComponent,
    EmptyComponent,
    SpinnerComponent,
    DimmerLoadingComponent,
    AlertMessageComponent,
    LocalFilesPickerComponent,
    FieldsMatcherComponent,
    LanguagesSwitchComponent,
    
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatRippleModule,
    MatButtonModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatMenuModule,
    MatDialogModule,
    MatSnackBarModule,
    QuillModule.forRoot(),
    AngularSvgIconModule.forRoot(),
    DragDropModule,
    TranslateModule,
    MatSelectModule,
    MatPaginatorModule,
    TranslateModule,
    MatAutocompleteModule


  ],
  exports:[
    FormBuilderComponent,
    DatatableComponent,
    DimmerLoadingComponent,
    EmptyComponent,
    OfflineComponent,
    SpinnerComponent,
    LocalFilesPickerComponent,
    LanguagesSwitchComponent,
    TranslateModule
  ],
  providers: [DatePipe]

})
export class SharedModule { }
