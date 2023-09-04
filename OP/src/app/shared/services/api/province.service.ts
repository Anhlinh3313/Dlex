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

export class ProvinceService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Province');
  }
  //
  async getProvinces(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('getProvinces', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public async getAllAsync(arrCols: string[] = [], pageSize: number = undefined, pageNumber: number = undefined): Promise<any> {
    const res = await this.getAll(arrCols, pageSize, pageNumber);
    if (res.isSuccess) {
      let data = res.data as any;
      data = SortUtil.sortAlphanumericalPram(data, 'name');
      return data;
    }
    return null;
  }

  public updateProvince(provinceId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('provinceId', provinceId);
    return super.getCustomApi('UpdateProvince', params);
  }

  getProvinceByName(name: string, countryId: number = null) {
    let params = new HttpParams();
    params = params.append('name', name);
    if (countryId) {
      params = params.append('countryId', countryId.toString());
    }

    return super.getCustomApi('getProvinceByName', params);
  }

  async getProvinceAsync(): Promise<SelectModel[]> {
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

}
