import { UserBaseModel } from "./base-user.model";
import { Role } from "./role.model";

export interface Admin extends UserBaseModel {
    role: Role | null;
}
 