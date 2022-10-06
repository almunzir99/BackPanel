import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/wrappers/api-response.model';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { AlertMessage, AlertMessageComponent, MessageTypes } from 'src/app/shared/components/alert-message/alert-message.component';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private _dialog: MatDialog) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(tap(evt => { }), catchError(
      (err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status == 400 && 'data' in err.error) {
            var apiError = err.error as ApiResponse<any>;
            this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
              data: {
                type: MessageTypes.ERROR,
                message: `${apiError.message}`,
                title: "Operation Failed",
                errors:apiError.errors
              }
            })
          }
          else if (err.status == 403) {
            this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
              data: {
                type: MessageTypes.ERROR,
                message: `Access Denied, Please Contact System Administrator`,
                title: "Forbidden"
              }
            })
          }
          else if(err.status == 401)
          {
            throw err;
          }
          else {
            this._dialog.open<AlertMessageComponent, AlertMessage>(AlertMessageComponent, {
              data: {
                type: MessageTypes.ERROR,
                message: `Failed to complete the operation of unkown reason`,
                title: "Operation Failed"
              }
            })
          }
        }
        throw err;
      }));
  }
}
