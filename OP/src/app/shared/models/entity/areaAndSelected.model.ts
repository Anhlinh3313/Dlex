import { Area } from "./area.model";
import { SelectModel } from "./Selected.model";

export class AreaAndSelect extends Area {
    multiSelectProvinces?: any[] = [];

    districts?: SelectModel[];
    multiSelectDistrict?: any[] = [];

    fromProvinces?: SelectModel[];
    fromDistricts?: SelectModel[];
    
    multiSelectFromProvince?: any[] = [];
    multiSelectFromDistrict?: any[] = [];

    showInfo: boolean;
    showEdit: boolean;
    filterFromProvince: string;
    filterToProvince: string;
    filterFromDistrict: string;
    filterToDistrict: string;
}
