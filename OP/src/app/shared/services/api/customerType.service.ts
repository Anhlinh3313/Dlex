import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerTypeService extends GeneralService{

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'CustomerType');
  }

  async getCustomerByFilter(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('getCustomerByFilter', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }
}
