import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Activity } from '../models/activity.model';
import { Admin } from '../models/admin.model';
import { PagedResponse } from '../models/wrappers/paged-response.model';
import { FieldSearchResult } from 'src/app/shared/components/datatable/datatable.component';

@Injectable({
  providedIn: 'root'
})
export class AdminsService {
  private moduleBaseUrl = ``;
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/admins/`
  }
  get(pageIndex = 1, pageSize = 10, searchValue = "", orderBy = "lastUpdate", ascending = false, list: FieldSearchResult[] = []): Observable<PagedResponse<Admin[]>> {
    var params: any = {
      PageIndex: pageIndex,
      PageSize: pageSize,
      orderBy: orderBy,
      ascending: ascending,
      title: searchValue
    }
    list.forEach((element,index) => {
      params[`expressions[${index}].propName`] = element.propName;
      params[`expressions[${index}].propValue`] = element.propValue;
      params[`expressions[${index}].operator`] = element.operator;

    });
    return this.http.get(`${this.moduleBaseUrl}`, { params: params }) as Observable<PagedResponse<Admin[]>>;
  }
  getActivities(pageIndex = 1, pageSize = 5): Observable<PagedResponse<Activity[]>> {
    var params: any = {
      PageIndex: pageIndex,
      PageSize: pageSize,
    }
    return this.http.get(`${this.moduleBaseUrl}activities`, { params: params }) as Observable<PagedResponse<Activity[]>>;

  }
  getAdminActivities(userId: number, pageIndex = 1, pageSize = 10): Observable<PagedResponse<Activity[]>> {
    var params: any = {
      PageIndex: pageIndex,
      PageSize: pageSize,
    }
    return this.http.get(`${this.moduleBaseUrl}${userId}/activities`, { params: params }) as Observable<PagedResponse<Activity[]>>;

  }
  post(admin: Admin) {
    console.log(admin);
    return this.http.post(`${this.moduleBaseUrl}register`, admin);
  }
  postAll(items: any[]) {
    return this.http.post(`${this.moduleBaseUrl}all`, items);
  }
  put(admin: Admin) {
    return this.http.put(`${this.moduleBaseUrl}${admin.id}`, admin);
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
  activeToggle(id: number) {
    return this.http.get(`${this.moduleBaseUrl}active?id=${id}`);
  }

}
