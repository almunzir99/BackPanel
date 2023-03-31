import { Status } from "../enums/status.enum";

export interface BaseModel {
   id?: number;
   status?: Status,
   createdAt?: string;
   lastUpdate?: string;
}