import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilderComponent } from './components/form-builder/form-builder.component';
import {ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSelectModule} from '@angular/material/select';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { QuillModule } from 'ngx-quill';
import { DatatableComponent } from './components/datatable/datatable.component';
import {MatPaginatorModule} from '@angular/material/paginator'; 
import {MatMenuModule} from '@angular/material/menu';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { OfflineComponent } from './components/placeholders/offline/offline.component';
import { EmptyComponent } from './components/placeholders/empty/empty.component';
import { SpinnerComponent } from './components/placeholders/spinner/spinner.component';
 

@NgModule({ 
  declarations: [
    FormBuilderComponent,
    DatatableComponent,
    OfflineComponent,
    EmptyComponent,
    SpinnerComponent
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
    MatSelectModule,
    MatDatepickerModule,
    MatPaginatorModule,
    MatMenuModule,
    QuillModule.forRoot(),
    AngularSvgIconModule.forRoot()

  ],
  exports:[
    FormBuilderComponent,
    DatatableComponent,
  ]
})
export class SharedModule { }
