import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { Service } from '../../models/entity/service.model';
import { FilterViewModel } from '../../models/viewModel/filter.viewModel';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class ServiceService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Service');
  }

  async getListService(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListService', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getAllServiceAsync(): Promise<SelectModel[]> {
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
      return selectModel;
    }
  }

  async serviceBySearchCode(model: FilterViewModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('ServiceBySearchCode', model);
    return res;
  }
}
