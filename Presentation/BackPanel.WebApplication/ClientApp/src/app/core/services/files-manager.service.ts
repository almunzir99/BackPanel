import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DirectoryModel } from '../models/directory.model';
import { FileModel } from '../models/file.models';
import { ApiResponse } from '../models/wrappers/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class FilesManagerService {
  private moduleBaseUrl = ``;
  constructor(private http: HttpClient, @Inject("BASE_API_URL") private baseUrl: string) {
    this.moduleBaseUrl = `${this.baseUrl}api/files-manager/`;
  }
  getDirectories(path?: string): Observable<ApiResponse<DirectoryModel[]>> {
    var params: any = {};
    if (path && path != '') params['path'] = path;
    return this.http.get<ApiResponse<DirectoryModel[]>>(`${this.moduleBaseUrl}directories`, { params: params });
  }
  getFiles(path?: string): Observable<ApiResponse<FileModel[]>> {
    var params: any = {};
    if (path) params['path'] = path;
    return this.http.get<ApiResponse<FileModel[]>>(`${this.moduleBaseUrl}files`, { params: params });
  }
  createDirectory(name:string,path?:string)
  {
    var params: any = {};
    if (path) params['path'] = path;
    if (name) params['directoryName'] = name;
    return this.http.post(`${this.moduleBaseUrl}directories`,{},{params:params});
  }
  deleteDirectory(name:string,path?:string)
  {
    var params: any = {};
    if (path) params['path'] = path;
    if (name) params['directoryName'] = name;
    return this.http.delete(`${this.moduleBaseUrl}directories`,{params:params});
  }
  renameDir(name:string,newName:string,path?:string)
  {
    var params: any = {};
    if (path) params['path'] = path;
    if (name) params['oldName'] = name;
    if (newName) params['newName'] = newName;
    return this.http.get(`${this.moduleBaseUrl}directories/rename`,{params:params});
  }
  moveDirectory(directoryName:string,oldPath?:string,newPath?:string)
  {
    var params: any = {};
    if (directoryName) params['directoryName'] = directoryName;
    if (oldPath) params['oldPath'] = oldPath;
    if (newPath) params['newPath'] = newPath;
    return this.http.get(`${this.moduleBaseUrl}directories/move`,{params:params});
  }
  deleteFile(name:string,path?:string)
  {
    var params: any = {};
    if (path) params['path'] = path;
    if (name) params['fileName'] = name;
    return this.http.delete(`${this.moduleBaseUrl}files`,{params:params});
  }
  renameFile(name:string,newName:string,path?:string)
  {
    var params: any = {};
    if (path) params['path'] = path;
    if (name) params['oldName'] = name;
    if (newName) params['newName'] = newName;
    return this.http.get(`${this.moduleBaseUrl}files/rename`,{params:params});
  }
  moveFile(fileName:string,oldPath?:string,newPath?:string)
  {
    var params: any = {};
    if (fileName) params['fileName'] = fileName;
    if (oldPath) params['oldPath'] = oldPath;
    if (newPath) params['newPath'] = newPath;
    return this.http.get(`${this.moduleBaseUrl}files/move`,{params:params});
  }
  uploadFiles(files:File[],path?:string){

    var formData = new FormData();
     for (let index = 0; index < files.length; index++) {
       formData.append("files",files[index]);
     }
     var params: any = {};
     if (path) params['path'] = path;
   return this.http.post(`${this.moduleBaseUrl}upload`,formData,{
     reportProgress: true,
     observe: 'events',
     params : params
   });
 
  }
}
