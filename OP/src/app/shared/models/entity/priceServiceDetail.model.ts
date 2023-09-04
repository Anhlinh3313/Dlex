import { Area } from './area.model';
import { EntityBasic } from './EntityBasic.model';
import { PriceService } from './priceService.model';
import { Weight } from './weight.model';

export class PriceServiceDetail extends EntityBasic {
    priceServiceId: number;
    priceService: PriceService;
    weightId: number;
    weight: Weight;
    areaId: number;
    area: Area;
    price: number;
}
