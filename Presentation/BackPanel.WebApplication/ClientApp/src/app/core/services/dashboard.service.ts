import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/wrappers/api-response.model';
import { counters } from 'src/app/core/models/counters.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private _baseModuleUrl: string = '';

  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this._baseModuleUrl = `${baseUrl}api/dashboard/`;
  }
  getCounters(): Observable<ApiResponse<counters>> {
    return this.http.get(`${this._baseModuleUrl}counters`) as Observable<ApiResponse<counters>>;
  }
}
