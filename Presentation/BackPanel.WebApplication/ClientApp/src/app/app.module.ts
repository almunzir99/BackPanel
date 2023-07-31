import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule, MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { AppRoutingModule } from './app-routing.module';
import { MatNativeDateModule, MAT_RIPPLE_GLOBAL_OPTIONS, RippleGlobalOptions } from '@angular/material/core';
import { QuillModule } from 'ngx-quill';
import { environment } from 'src/environments/environment';
import { MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MAT_COLOR_FORMATS, NGX_MAT_COLOR_FORMATS } from '@angular-material-components/color-picker';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';


const globalRippleConfig: RippleGlobalOptions = {
  disabled: false,
  animation: {
    enterDuration: 700,
    exitDuration: 500
  }
};
@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatNativeDateModule,
    MatDialogModule,
    AngularSvgIconModule.forRoot(),
    QuillModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      },
  })

  ],
  providers: [
    { provide: MAT_RIPPLE_GLOBAL_OPTIONS, useValue: globalRippleConfig },
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } },
    { provide: "BASE_API_URL", useValue: environment.baseUrl },
    { provide: "DIRECTION", useValue: environment.direction },

    { provide: MAT_DIALOG_DATA, useValue: undefined },
    {
      provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true
    },
    {
      provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true
    },
    { provide: MAT_COLOR_FORMATS, useValue: NGX_MAT_COLOR_FORMATS }



  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, `${environment.baseUrl}assets/contents/`, '.json');;
}



