import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { GeneralService } from './general.service';
import { HttpParams } from '@angular/common/http';
import { MsgService } from '../local/msg.service';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { PriceService } from '../../models/entity/priceService.model';

@Injectable({
  providedIn: 'root'
})
export class PriceServiceService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PriceService');
  }

  public async getByCode(code: string): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('code', code);
    const res = await this.getCustomApi('GetByCode', params) as ResponseModel;
    return res;
  }

  async getByCodeAsync(code: string): Promise<PriceService[]>{
    const res = await this.getByCode(code);
    if (!this.isValidResponse(res)) { return; }
    const data = res.data as PriceService[];
    return data;
  }

  async getListPriceService(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListPriceService', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public CopyPriceServiceAsync(priceServiceId: any, newPriceServiceCode: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("priceServiceId", priceServiceId);
    params = params.append("newCode", newPriceServiceCode);
    return super.getCustomApi("CopyPriceService", params);
  }
}
