export class MenuItem{
    title?:string;
    icon?:string;
    route?:string;
    open?:boolean = false;
    permissionName?:string;
    allowedRoles?:string[];
    children?:MenuItem[];
}