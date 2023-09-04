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

export class AccountBankService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'AccountBank');
  }

  async getListBankAccount(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListBankAccount', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public getBankAll(): Promise<ResponseModel> {
    let params = new HttpParams();
    return super.getCustomApi('GetBankAll',  params);
  }

  async getBankAllAsync(): Promise<SelectModel[]> {
    const res = await this.getBankAll();
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];
      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }

  public getBranchBy(bankId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("bankId", bankId);
    return super.getCustomApi('GetBranchBy',  params);
  }

  async getBranchByAsync(bankId: any): Promise<SelectModel[]> {
    const res = await this.getBranchBy(bankId);
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];
      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }
}
