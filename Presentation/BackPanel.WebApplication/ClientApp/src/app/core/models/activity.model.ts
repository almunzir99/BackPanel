import { Admin } from "./admin.model";
import { BaseModel } from "./base.model";

 export interface Activity extends BaseModel {
    adminId: number;
    admin: Admin | null;
    effectedTable: string | null;
    effectedRowId: number;
    action: string | null;
}