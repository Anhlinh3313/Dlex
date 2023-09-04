import { EntityBasic } from './EntityBasic.model';
import { RolePage } from './rolePage.model';

export class Page extends EntityBasic {
    parentPageId: number;
    aliasPath: string;
    pageOrder: number;
    isAccess: boolean;
    isAdd: boolean;
    isEdit: boolean;
    isDelete: boolean;
    modulePageId: number;
    icon: string;
    display: string;
    children: Page[] = [];
    rolePage: RolePage;
    active: string;
    background: string;
    childNumber?: number = 0;
    isCheckAll?: boolean;
}
