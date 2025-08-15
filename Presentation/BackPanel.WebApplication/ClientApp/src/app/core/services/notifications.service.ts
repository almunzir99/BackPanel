import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiNotification } from '../models/api-notification.model';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private moduleBaseUrl = ``;
  public $notifications = new BehaviorSubject<ApiNotification[]>([]);
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/notifications/`;
  }

  getNotifications(): Observable<ApiResponse<ApiNotification[]>> {
    return this.http.get(`${this.moduleBaseUrl}?userType=Admin`) as Observable<ApiResponse<ApiNotification[]>>;

  }
  readNotifications(): Observable<ApiResponse<ApiNotification[]>> {
    return this.http.get(`${this.moduleBaseUrl}unread?autoRead=true&userType=Admin`) as Observable<ApiResponse<ApiNotification[]>>;

  }
}
