import { EntityBasic } from './EntityBasic.model';
import { PromotionDetail } from './promotionDetail.model';

export class Promotion extends EntityBasic {
    promotionTypeId: number;
    promotionTypeName: string;
    promotionNot: string;
    totalPromotion: any;
    totalCode: any;
    fromDate: any;
    toDate: any;
    isPublic: any;
    isHidden: any;
    rowNum: any;
    promotionDetails: PromotionDetail[];
    totalCount: number;
}
