import { EntityBasic } from './EntityBasic.model';

export class PromotionCustomer extends EntityBasic {
    promotionId: number;
    customerId: number;
    customerName: string;
    promotionCode: string;
    phoneNumber: string;
    rowNum: any;
    totalCount: number;
}
