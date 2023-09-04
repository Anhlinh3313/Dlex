import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class PromotionCustomerService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PromotionCustomer');
  }

  async getListPromotionCustomer(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListPromotionCustomer', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public updatePromotionCustomer(promotionCustomerId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("promotionCustomerId", promotionCustomerId);
    return super.getCustomApi("UpdatePromotionCustomer", params);
  }

  public createPromotionCustomer(customerId: number, promotionId: number): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("customerId", customerId+'');
    params = params.append("promotionId", promotionId+'');
    return super.getCustomApi("CreatePromotionCustomer", params);
  }

}
