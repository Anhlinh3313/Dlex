import { EntityBasic } from "./EntityBasic.model";

export class FromPrint extends EntityBasic {
    numOrder:number;
    isPublic:boolean;
    formPrintBody:string;
    formPrintTypeId:number;
    width: number;
    height: number;
}