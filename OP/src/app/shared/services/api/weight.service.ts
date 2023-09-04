import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class WeightService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Weight');
  }

  public getByWeightGroup(weightGroupId: number, priceServiceId: number): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('weightGroupId', weightGroupId + '');
    params = params.append('priceServiceId', priceServiceId + '');
    return super.getCustomApi('GetByWeightGroup', params);
  }
  async getByWeightGroupAsync(weightGroupId: any, priceServiceId?: any): Promise<any> {
    const res = await this.getByWeightGroup(weightGroupId, priceServiceId);
    if (!this.isValidResponse(res)) { return; }
    const data = res;
    return data;
  }
}
