import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from '../models/role.model';
import { PagedResponse } from '../models/wrappers/paged-response.model';

@Injectable({
  providedIn: 'root'
})
export class RolesService {
  private moduleBaseUrl = ``;

  constructor(private http: HttpClient, @Inject("BASE_API_URL") private baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/roles/`

   }
  get(pageIndex = 1, pageSize = 10, title = "", orderBy = "lastUpdate", ascending = false): Observable<PagedResponse<Role[]>> {
    var params:any = {
      PageIndex:pageIndex,
      PageSize:pageSize,
      orderBy:orderBy,
      ascending:ascending,
      title:title
    }
    return this.http.get(`${this.moduleBaseUrl}`,{params:params}) as Observable<PagedResponse<Role[]>>;
  }
  post(role: Role) {
    return this.http.post(`${this.moduleBaseUrl}`, role);
  }
  put(role: Role) {
    return this.http.put(`${this.moduleBaseUrl}${role.id}`, role);
  }
  delete(id: number) {
    return this.http.delete(`${this.moduleBaseUrl}${id}`);
  }
}
