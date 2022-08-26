import { BaseModel } from "./base.model";

export interface UserBaseModel extends BaseModel {
    username: string;
    email: string;
    phone: string;
    photo: string;
    token: string;
    notifications: Notification[];
}