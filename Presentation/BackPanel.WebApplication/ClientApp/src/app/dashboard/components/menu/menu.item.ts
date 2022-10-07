export class MenuItem{
    title?:string;
    icon?:string;
    route?:string;
    open?:boolean = false;
    permissionName?:string;
    children?:MenuItem[];
}