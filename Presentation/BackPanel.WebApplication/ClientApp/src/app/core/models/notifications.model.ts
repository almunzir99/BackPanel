import { BaseModel } from "./base.model";

export interface Notification extends BaseModel{
    title: string;
    message: string;
    action: string;
    module: string;
    url: string;
    read: boolean;
    groupedItem: number;
}

 