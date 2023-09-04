import { District } from "../entity/district.model";
import { Ward } from "../entity/ward.model";

export class DataHubRouteViewModel {
    selectedProvinceCiyIds:number[];
    selectedDistrictIds:number[];
    districts: District[];
    wards: Ward[];
}