import { EntityBasic } from './EntityBasic.model';
import { Formula } from './formula.model';
import { SelectModel } from './Selected.model';
import { Structure } from './structure.model';
import { WeightGroup } from './weightGroup.model';

export class Weight extends EntityBasic {
    weightGroupId?: number;
    weightGroup?: WeightGroup;
    weightFrom?: number;
    weightTo?: number;
    weightPlus?: number;
    isAuto?: boolean;
    isWeightCal?: boolean;
    structureId?: number;
    structure?: Structure;
    valueFrom?: number;
    valueTo?: number;
    priceServiceId?: number;
    pricingTypeId?: number;
    //
    formulaId: number;
    formula: Formula;
    //
    fakeId: any;
}
