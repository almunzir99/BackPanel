import { Activity } from "./activity.model";
import { UserBaseModel } from "./base-user.model";
import { Role } from "./role.model";

export interface Admin extends UserBaseModel {
    isManager: boolean;
    image: string | null;
    roleId: number | null;
    role: Role | null;
    activities: Activity[]; 
}
 