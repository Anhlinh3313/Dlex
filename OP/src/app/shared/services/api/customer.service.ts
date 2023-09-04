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
export class CustomerService extends GeneralService{

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Customer');
  }

  async getCustomerByFilter(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('getCustomerByFilter', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async getAllCustomerAsync(): Promise<SelectModel[]> {
    const res = await this.getAll();
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];
      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }

  public updateCustomerbyUserFail(customerId: number): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("customerId", customerId + "");
    return super.getCustomApi("UpdateCustomerbyUserFail", params);
  }

  public ranDomCodeCustomer(code: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("code", code);
    return super.getCustomApi("RanDomCodeCustomer", params);
  }
}
