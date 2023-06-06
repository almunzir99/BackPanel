import { BaseModel } from "./base.model";

export interface Image extends BaseModel {
    path: string | null;
}