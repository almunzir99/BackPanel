import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/wrappers/api-response.model';
import { Stats } from 'src/app/core/models/stats.model';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
 private _baseModuleUrl: string = '';

  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) { 
    this._baseModuleUrl = `${baseUrl}api/statistics`;
  }
  getStats() : Observable<ApiResponse<Stats>>{
      return this.http.get(`${this._baseModuleUrl}`) as Observable<ApiResponse<Stats>> ;
  }
}
