import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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
      'PaginationFilter.PageIndex': pageIndex,
      'PaginationFilter.PageSize': pageSize,
      OrderBy: orderBy,
      Descending: !ascending,
      Search: searchValue
    };
    list.forEach((element, index) => {
      params[`SearchExpressions[${index}].PropName`] = element.propName;
      params[`SearchExpressions[${index}].PropValue`] = element.propValue;
      params[`SearchExpressions[${index}].Operator`] = element.operator;
    });
    return this.http.get(`${this.moduleBaseUrl}`, { params: params }) as Observable<PagedResponse<Admin[]>>;
  }
  post(admin: Admin) {
    return this.http.post(`${this.moduleBaseUrl}`, admin);
  }
  put(admin: Admin) {
    return this.http.put(`${this.moduleBaseUrl}${admin.id}`, admin);
  }
  delete(id: number) {
    return this.http.delete(`${this.moduleBaseUrl}${id}`);
  }
  exportExcel(next?: () => void, failed?: (err: any) => void) {
    this.http.get(`${this.moduleBaseUrl}export/excel`, { responseType: 'blob' }).subscribe({
      next: (res) => {
        const downloadURL = window.URL.createObjectURL(res);
        const link = document.createElement('a');
        link.href = downloadURL;
        link.download = 'admins.xlsx';
        link.click();
        if (next) next();
      },
      error: (error) => { if (failed) failed(error); }
    });
  }
  activeToggle(id: number) {
    return this.http.get(`${this.moduleBaseUrl}active`, { params: { id: id } });
  }
}
