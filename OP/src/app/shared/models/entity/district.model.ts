import { EntityBasic } from './EntityBasic.model';
import { Province } from './province.model';

export class District extends EntityBasic {
    lat: number;
    lng: number;
    companyName: string;
    provinceId: number;
    provinceName: string;
    isRemote = false;
    kmNumber: number;
    totalCount: number;
    province:Province;
}
