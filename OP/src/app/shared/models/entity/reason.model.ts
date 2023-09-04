export class Reason {
    id: number;
    concurrencyStamp: string;
    name: string;
    code: string;
    pickFail: boolean;
    pickCancel: boolean;
    deliverFail: boolean;
    deliverCancel: boolean;
    returnFail: boolean;
    returnCancel: boolean;
    isDelay: boolean;
    isIncidents: boolean;
    isPublic: boolean;
    isAcceptReturn: boolean;
    isMustInput: boolean;
    itemOrder: number;
    roleId: number;
    isUnlockListGood: boolean;
    isEnabled: boolean;
    rowNum: any;
    totalCount: number;
}
