import { EntityBasic } from './EntityBasic.model';

export class Company extends EntityBasic {
    phoneNumber: string;
    hotline: string;
    email: string;
    address: string;
    website: string;
    companySizeId: number;
    prefixShipmentNumber: string;
    prefixRequestNumber: string;
    topUp: number;
    logoUrl: string;
    contactName: string;
    contactPhone: string;
}
