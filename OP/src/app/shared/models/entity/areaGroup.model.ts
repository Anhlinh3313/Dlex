import { EntityBasic } from "./EntityBasic.model";
import { Hub } from "./hub.model";

export class AreaGroup extends EntityBasic {
    hubId: number;
    hub: Hub;
    isAuto: boolean;
}