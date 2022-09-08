import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Admin } from '../models/admin.model';
import { AuthenticationModel } from '../models/authentication.model';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private moduleBaseUrl = ``;
  private authTokenKey = "auth_token";
  private _$currentUser = new BehaviorSubject<Admin | null>(null);
  public get $currentUser() {
    return this._$currentUser;
  }
  setCurrentUser(value:Admin) {
    this._$currentUser.next(value);
  }
  constructor(private http: HttpClient, @Inject("BASE_API_URL")  baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/admins/`;
  }
  autthenticate(model:AuthenticationModel): Observable<ApiResponse<Admin>> {
    return this.http.post(`${this.moduleBaseUrl}authenticate`,model) as Observable<ApiResponse<Admin>>;
  }
  getCurrentUser() : Observable<ApiResponse<Admin>> {
    return this.http.get(`${this.moduleBaseUrl}profile`) as Observable<ApiResponse<Admin>>;

  } 
  saveToken(token: string) {
    localStorage.setItem(this.authTokenKey, token);
  }
  getToken = (): string | null => localStorage.getItem(this.authTokenKey);

  logout() {
    localStorage.removeItem(this.authTokenKey);
  }
}
