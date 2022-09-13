import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Message } from '../models/message.model';
import { PagedResponse } from '../models/wrappers/paged-response.model';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  private moduleBaseUrl = ``;
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/messages/`
   }
  get(pageIndex = 1, pageSize = 10,searchValue="",orderBy="lastUpdate",ascending = false): Observable<PagedResponse<Message[]>> {
    var params:any = {
      PageIndex:pageIndex,
      PageSize:pageSize,
      orderBy:orderBy,
      ascending:ascending,
      title:searchValue
    }
    return this.http.get(`${this.moduleBaseUrl}`,{params:params}) as Observable<PagedResponse<Message[]>>;
  }
}
