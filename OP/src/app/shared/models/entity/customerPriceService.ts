import { Customer } from './customer.model';
import { EntityBasic } from './EntityBasic.model';

export class CustomerPriceServiceModel extends EntityBasic {
    customerId: number;
    priceServiceId: number;
    vatPercent: number;
    fuelPercent: number;
    dim: number;
    remoteAreasPricePercent: number;
    customer: Customer;
}
