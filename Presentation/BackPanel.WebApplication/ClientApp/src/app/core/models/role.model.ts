import { BaseModel } from "./base.model";
import { PermissionClaim } from "./permission-claim.model";

export interface Role extends BaseModel{
    title: string;
    permissions?: PermissionClaim[];
}