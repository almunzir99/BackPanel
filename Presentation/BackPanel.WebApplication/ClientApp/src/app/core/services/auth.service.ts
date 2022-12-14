import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Admin } from '../models/admin.model';
import { ApiNotification } from '../models/api-notification.model';
import { AuthenticationModel } from '../models/authentication.model';
import { Role } from '../models/role.model';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private moduleBaseUrl = ``;
  private authTokenKey = "auth_token";
  private _$currentUser = new BehaviorSubject<Admin | null>(null);
  public $notifications = new BehaviorSubject<ApiNotification[]>([]);
  public $role = new BehaviorSubject<Role | null>(null);
  public get $currentUser() {
    return this._$currentUser;
  }
  setCurrentUser(value: Admin) {
    this._$currentUser.next(value);
  }
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/admins/`;
  }
  autthenticate(model: AuthenticationModel): Observable<ApiResponse<Admin>> {
    return this.http.post(`${this.moduleBaseUrl}authenticate`, model) as Observable<ApiResponse<Admin>>;
  }
  getCurrentUser(): Observable<ApiResponse<Admin>> {
    return this.http.get(`${this.moduleBaseUrl}profile`) as Observable<ApiResponse<Admin>>;

  }
  updateProfile(admin: Admin): Observable<ApiResponse<Admin>> {
    return this.http.put(`${this.moduleBaseUrl}${admin.id}`, admin) as Observable<ApiResponse<Admin>>;
  }
  changePassword(oldPassword: string, newPassword: string) {
    return this.http.put(`${this.moduleBaseUrl}profile/password-reset`, { newPassword: newPassword, oldPassword: oldPassword });

  }
  getNotifications(): Observable<ApiResponse<ApiNotification[]>> {
    return this.http.get(`${this.moduleBaseUrl}notifications`) as Observable<ApiResponse<ApiNotification[]>>;

  }
  readNotifications(): Observable<ApiResponse<ApiNotification[]>> {
    return this.http.get(`${this.moduleBaseUrl}notifications/unread?autoRead=true`) as Observable<ApiResponse<ApiNotification[]>>;

  }
  
  saveToken(token: string) {
    localStorage.setItem(this.authTokenKey, token);
  }
  getToken = (): string | null => localStorage.getItem(this.authTokenKey);

  logout() {
    localStorage.removeItem(this.authTokenKey);
    this.$currentUser.next(null);
  }
}
