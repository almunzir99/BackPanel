import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class TranslationEditorService {
  private moduleBaseUrl = ``;
  constructor(private http: HttpClient, @Inject("BASE_API_URL") baseUrl: string) {
    this.moduleBaseUrl = `${baseUrl}api/translations/`
   }
   getLanguages() : Observable<ApiResponse<string[]>>
   {
      return this.http.get(`${this.moduleBaseUrl}languages`) as  Observable<ApiResponse<string[]>>;
   }
   getLanguageTree() : Observable<ApiResponse<any>>
   {
      return this.http.get(`${this.moduleBaseUrl}tree`) as  Observable<ApiResponse<any>>;
   }
   createNode(parent:string,title:string,values:any) :  Observable<ApiResponse<any>>{
      return this.http.post(`${this.moduleBaseUrl}nodes/new`,{parent,title,values}) as  Observable<ApiResponse<any>>;
   }
   
   updateNode(parent:string,title:string,values:any) :  Observable<ApiResponse<any>>{
      return this.http.put(`${this.moduleBaseUrl}nodes/update`,{parent,title,values}) as  Observable<ApiResponse<any>>;
   }
   deleteNode(parent:string,title:string) :  Observable<ApiResponse<any>>{
      return this.http.delete(`${this.moduleBaseUrl}nodes/delete?parent=${parent}&title=${title}`,{}) as  Observable<ApiResponse<any>>;
   }
   createParent(title:string) :  Observable<ApiResponse<any>>{
      return this.http.post(`${this.moduleBaseUrl}parents/new?title=${title}`,{}) as  Observable<ApiResponse<any>>;
   }
   updateParent(oldtitle:string,newTitle:string) :  Observable<ApiResponse<any>>{
      return this.http.put(`${this.moduleBaseUrl}parents/update?oldTitle=${oldtitle}&newTitle=${newTitle}`,{}) as  Observable<ApiResponse<any>>;
   }
   deleteParent(title:string) :  Observable<ApiResponse<any>>{
      return this.http.delete(`${this.moduleBaseUrl}parents/delete?title=${title}`,{}) as  Observable<ApiResponse<any>>;
   }
   CreateLanguage(code:string) :  Observable<ApiResponse<any>>{
      return this.http.post(`${this.moduleBaseUrl}languages/new?code=${code}`,{}) as  Observable<ApiResponse<any>>;
   }
}
