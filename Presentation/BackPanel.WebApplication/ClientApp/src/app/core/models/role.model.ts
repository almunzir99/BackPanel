import { BaseModel } from "./base.model";
import { Permission } from "./permission.model";

export interface Role extends BaseModel{
    title: string;
    messagesPermissions: Permission;
    adminsPermissions: Permission;
    rolesPermissions: Permission;
}