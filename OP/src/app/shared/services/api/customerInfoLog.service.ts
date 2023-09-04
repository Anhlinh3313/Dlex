import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { CustomerInfoLog } from '../../models/entity/customerInfoLog.model';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerInfoLogService extends GeneralService{

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'CustomerInfoLog');
  }

  async getListCustomerInfoLogr(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('GetListCustomerInfoLog', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async createOrUpdateImportExcel(model: CustomerInfoLog[]) {
    const res = await this.postCustomApi('CreateOrUpdateImportExcel', model);
    return res;
  }

}
