import { District } from './district.model';
import { EntityBasic } from './EntityBasic.model';

export class Ward extends EntityBasic {
    lat: number;
    lng: number;
    companyName: string;
    provinceId: number;
    provinceName: string;
    districtId: number;
    districtName: string;
    isRemote = false;
    kmNumber: number;
    totalCount: number;
    district: District;
}
