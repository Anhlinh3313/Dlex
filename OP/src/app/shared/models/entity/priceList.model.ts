import { EntityBasic } from './EntityBasic.model';
import { Hub } from './Hub.model';

export class PriceList extends EntityBasic {

    hubId: number;
    hub: Hub;
    priceListDVGTName: string;
    hubName: string;
    fuelSurcharge: any;
    remoteSurcharge: any;
    publicDateFrom: any;
    publicDateTo: any;
    numOrder: number;
    isPublic: any;
    rowNum: any;
    totalCount: any;
    priceListDVGTId: number;
    VATSurcharge: number;
}
