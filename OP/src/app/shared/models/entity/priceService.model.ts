import { Service } from './service.model';
import { EntityBasic } from './EntityBasic.model';


export class PriceService extends EntityBasic {

    priceListId: number;
    areaGroupId: number;
    weightGroupId: number;
    serviceId: number;
    service: Service;
    isAuto: boolean;
    vatPercent: number;
    fuelPercent: number;
    dim: number;
    remoteAreasPricePercent: number;
    structureId: number;
    pricingTypeId: number;
    numOrder: number;

    isTwoWay = false;
    isPublic = false;
    isKeepWeight = false;

    publicDateFrom: any;
    publicDateTo: any;

    vatSurcharge: number;
    areaGroup: any;
    weightGroup: any;
    priceList: any;
    totalCount: number;
}
