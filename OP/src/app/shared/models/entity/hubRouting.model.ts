import { Hub } from "./hub.model";
import { User } from "./user.model";

export class HubRouting {
    name: string;
    code: string;
    hubId: number;
    userId: number;
    codeConnect?: string;
    radiusServe?: any;
    isTruckDelivery: boolean;
    wardIds: number[];
    streetJoinIds?: number[];
    concurrencyStamp: string;
    isEnabled: boolean = true;
    id: number;
    hub?: Hub;
    user?: User;
}