import { Injectable } from '@angular/core';
import { GeneralService } from './general.service';
import { MsgService } from '../local/msg.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CustomerPriceServiceModel } from '../../models/entity/customerPriceService';

@Injectable({
  providedIn: 'root'
})
export class CustomerPriceServiceService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'CustomerPriceService');
  }

  async GetByPriceServiceId(priceServiceId): Promise<CustomerPriceServiceModel[]> {
    let params = new HttpParams();
    params = params.append('priceServiceId', priceServiceId);

    const res = await this.getCustomApi('GetByPriceServiceId', params);
    if (!this.isValidResponse(res)) { return; }
    return res.data as CustomerPriceServiceModel[];
  }
}
