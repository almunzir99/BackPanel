export interface ApiResponse<T>{
    data:T;
    success:string;
    message:string;
    errors:string[];

}