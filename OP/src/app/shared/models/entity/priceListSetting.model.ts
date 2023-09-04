import { EntityBasic } from './EntityBasic.model';

export class PriceListSetting extends EntityBasic {
    priceListCode: string;
    customerCode: string;
    customerName: string;
    customerId: number;
    serviceId: number;
    priceListId: number;
    vatSurcharge: any; 
    fuelSurcharge: any; 
    vsvxSurcharge: any; 
    dim: any;
    rowNum: any;
    totalCount: number;
}