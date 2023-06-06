import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { CompanyInfo } from '../models/company-info.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyInfoService {
  private moduleBaseUrl = ``;
  private _$companyIfo = new BehaviorSubject<CompanyInfo | null>(null);
  public get $companyIfo() {
    return this._$companyIfo.value;
  }
  public setCompanyInfo(value:CompanyInfo) {
    this._$companyIfo.next(value);
  }
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/companyInfos/`;
  }
  single() : Observable<ApiResponse<CompanyInfo>> {
    return this.http.get(`${this.moduleBaseUrl}single`) as Observable<ApiResponse<CompanyInfo>>;
  }
  put(item: CompanyInfo) : Observable<ApiResponse<CompanyInfo>>{
    return this.http.put(`${this.moduleBaseUrl}${item.id}`, item) as Observable<ApiResponse<CompanyInfo>>;
  }
}
