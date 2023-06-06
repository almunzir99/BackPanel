import { BaseModel } from "./base.model";
import { Image } from "./image.model";
export interface CompanyInfo extends BaseModel {
    companyName: string | null;
    address: string | null;
    logoId: number;
    logo: Image | null;
    email: string | null;
    phoneNumber: string | null;
    fax: string | null;
    deliveryCompanyId:number | null;
    aboutUs:string | null;
}