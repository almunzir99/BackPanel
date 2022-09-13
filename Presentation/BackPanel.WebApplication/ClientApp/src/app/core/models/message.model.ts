import { BaseModel } from "./base.model";

export interface Message extends BaseModel{
    fullName:string;
    email:string;
    content:string;
    phone:string;
}