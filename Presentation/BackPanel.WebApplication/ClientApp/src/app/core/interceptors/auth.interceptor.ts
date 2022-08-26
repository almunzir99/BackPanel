import { Inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(@Inject("BASE_API_URL") private _baseUrl: string,private _authService:AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    var token = this._authService.getToken();
    var isApiUrl = request.url.startsWith(this._baseUrl);
    console.log(token);
    console.log(isApiUrl);

    if(token && isApiUrl)
    {
        request = request.clone({
          setHeaders : {
             Authorization: `Bearer ${token}`
          } 
        })
    }
    return next.handle(request);
  }
}
