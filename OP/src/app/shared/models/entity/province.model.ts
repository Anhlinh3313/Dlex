import { EntityBasic } from './EntityBasic.model';

export class Province extends EntityBasic {
    lat: number;
    lng: number;
    companyId: number;
    companyName: string;
    countryId: number;
    countryName: string;
    totalCount: number;
}
