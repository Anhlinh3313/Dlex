import { EntityBasic } from './EntityBasic.model';

export class AccountBank extends EntityBasic {
    branchId: number;
    bankId: number;
    branchName: string;
    rowNum: any;
    totalCount: number;
}
