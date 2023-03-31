import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from '../models/role.model';
import { ApiResponse } from '../models/wrappers/api-response.model';
import { PagedResponse } from '../models/wrappers/paged-response.model';

@Injectable({
  providedIn: 'root'
})
export class RolesService {
  private moduleBaseUrl = ``;

  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
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
  single(id: number) : Observable<ApiResponse<Role>> {
    return this.http.get(`${this.moduleBaseUrl}${id}`) as  Observable<ApiResponse<Role>> ;
  }
  delete(id: number) {
    return this.http.delete(`${this.moduleBaseUrl}${id}`);
  }
  export(type: string, next?: () => void, failed?: (err: any) => void) {
    this.http.get(`${this.moduleBaseUrl}export/${type}`, { responseType: 'blob' }).subscribe(res => {
      let blob = new Blob([res], { type: 'text/plain' });
      var downloadURL = window.URL.createObjectURL(res);
      var link = document.createElement('a');
      link.href = downloadURL;
      var ext = (type == 'excel') ? '.xlsx' : '.pdf';
      link.download = `data${ext}`;
      link.click();
      if (next)
        next();
    }, error => { if (failed) failed(error) })
  }
  postAll(items: any[]) {
    return this.http.post(`${this.moduleBaseUrl}all`, items);
  }
  
}
