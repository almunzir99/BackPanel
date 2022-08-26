import { BaseModel } from "./base.model";

export interface Permission extends BaseModel {
    create: boolean;
    read: boolean;
    update: boolean;
    delete: boolean;
}