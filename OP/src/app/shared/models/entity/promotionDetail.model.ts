import { SelectModel } from "./Selected.model";

export class PromotionDetail {
  fakeId?: number;
  id?: number;
  code?: string;
  name?:string;
  promotionId?:number;
  promotionFormulaId?:number;
  value?:number =0;
  valueFrom?:number =0;
  valueTo?:number =0;
  promotionDetailServiceDVGTs?:PromotionDetailServiceDVGTs[]=[];
  serviceDVGTs?:any[]=[];
  totalCount?:number = 0;
  promotionFormulaName?: string;
  dataServiceDVGTs?: SelectModel[]=[];
  selectedItemsLabel?: any;
  concurrencyStamp?: string;
}

export class PromotionDetailServiceDVGTs{
  id?:number;
  isEnabled?:boolean;
  promotionDetailId?:number;
  serviceId?: number;
}
