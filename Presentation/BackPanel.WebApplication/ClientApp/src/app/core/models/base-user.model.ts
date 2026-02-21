import { BaseModel } from "./base.model";

export interface UserBaseModel extends BaseModel {
    userName: string;
    email: string;
    phoneNumber: string;
    token: string;
    image: string | null;
    isManager: boolean;
    roleId: number | null;
}