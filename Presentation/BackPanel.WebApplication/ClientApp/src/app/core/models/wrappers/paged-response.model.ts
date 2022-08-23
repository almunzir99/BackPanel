import { ApiResponse } from "./api-response.model";

export interface PagedResponse<T> extends ApiResponse<T>{
    pageIndex:number;
    totalPages:number;
    totalRecords:number;
    firstPage:string;
    lastPage:string;
    nextPage:string;
    previousPage:string;
    
}