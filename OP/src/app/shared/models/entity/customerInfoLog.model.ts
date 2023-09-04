import { EntityBasic } from './EntityBasic.model';
import { IsError } from './isError.model';

export class CustomerInfoLog extends EntityBasic {
    wardId: number;
    districtId: number;
    provinceId?: number;
    phoneNumber: string;
    address: string;
    addressNote: string;
    provinceName: string;
    districtName: string;
    wardName: string;
    companyName: string;
    totalCount: number;
    isError: IsError = new IsError();
    fakeId: any;
}
