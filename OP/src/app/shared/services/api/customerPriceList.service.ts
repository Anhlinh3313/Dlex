import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerPriceListService extends GeneralService{

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'CustomerPriceList');
  }

  async getPriceListByCustomerId(customerId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('customerId', customerId);
    params = params.append('cols', 'PriceList');
    const res = await this.getCustomApi('GetPriceListByCustomerId', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }
}
