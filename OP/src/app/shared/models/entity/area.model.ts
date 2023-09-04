import { EntityBasic } from "./EntityBasic.model";

export class Area extends EntityBasic {
    expectedTime: any;
    fakeId?: number;
    areaGroupId: number;
    areaGroup: any;
    districtIds: number[];
    provinceIds: number[];
    district: any;
    province: any;
    fromProvinceIds: number[];
    isAuto: boolean;
    priceServiceId: number;
}
