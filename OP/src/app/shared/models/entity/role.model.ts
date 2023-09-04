import { EntityBasic } from './EntityBasic.model';

export class Role extends EntityBasic {
    normalizedName: string;
    parrentRoleId: number;
    parrentRoleName: string;
    companyId: number;
    companyName: string;
    totalCount: number;
}
