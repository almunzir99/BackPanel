import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Admin } from '../models/admin.model';
import { PagedResponse } from '../models/wrappers/paged-response.model';

@Injectable({
  providedIn: 'root'
})
export class AdminsService {
  private moduleBaseUrl = ``;
  constructor(private http: HttpClient, @Inject("BASE_API_URL") private baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/admins/`
   }
  get(pageIndex = 1, pageSize = 10,searchValue="",orderBy="lastUpdate",ascending = false): Observable<PagedResponse<Admin[]>> {
    var params:any = {
      PageIndex:pageIndex,
      PageSize:pageSize,
      orderBy:orderBy,
      ascending:ascending,
      title:searchValue
    }
    return this.http.get(`${this.moduleBaseUrl}`,{params:params}) as Observable<PagedResponse<Admin[]>>;
  }
  post(admin: Admin) {
    console.log(admin);
    return this.http.post(`${this.moduleBaseUrl}register`, admin);
  }
  put(admin: Admin) {
    return this.http.put(`${this.moduleBaseUrl}${admin.id}`, admin);
  }
  delete(id: number) {
    return this.http.delete(`${this.moduleBaseUrl}${id}`);
  }
}
