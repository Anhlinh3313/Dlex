import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { GetListFormPrintViewModel } from '../../models/viewModel/getListFormPrint.viewModel';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class FormPrintService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'FormPrint');
  }

  public getFormPrintType(): Promise<ResponseModel> {
    let params = new HttpParams();
    return super.getCustomApi('GetFormPrintType',  params);
  }

  async getFormPrintTypeAsync(): Promise<SelectModel[]> {
    const res = await this.getFormPrintType();
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

  public getFormPrint(typeId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("typeId", typeId);
    return super.getCustomApi('GetFormPrintByType',  params);
  }

  async getFormPrintAsync(typeId: any): Promise<SelectModel[]> {
    const res = await this.getFormPrint(typeId);
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

  public getListFormPrint(model: GetListFormPrintViewModel): Promise<ResponseModel> {
    return super.postCustomApi('GetListFormPrint',  model);
  }

}
