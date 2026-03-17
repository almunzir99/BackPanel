import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Business } from '../models/business.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class BusinessService {
  private moduleBaseUrl = ``;
  private _$business = new BehaviorSubject<Business | null>(null);
  public get $companyIfo() {
    return this._$business.value;
  }
  public setBusiness(value: Business) {
    this._$business.next(value);
  }
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/business/`;
  }
  single() : Observable<ApiResponse<Business>> {
    return this.http.get(`${this.moduleBaseUrl}single`) as Observable<ApiResponse<Business>>;
  }
  put(item: Business) : Observable<ApiResponse<Business>>{
    return this.http.put(`${this.moduleBaseUrl}${item.id}`, item) as Observable<ApiResponse<Business>>;
  }
}
