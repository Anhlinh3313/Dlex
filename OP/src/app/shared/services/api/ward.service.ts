import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { Ward } from '../../models/entity/ward.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})

export class WardService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Ward');
  }
  //
  async getWards(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('getWards', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getWardsSelectModelAsync(viewModel: any): Promise<SelectModel[]> {
    const res = await this.getWards(viewModel);
    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn dữ liệu --', data: null, value: null }];
      const datas = res.data as any[];
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

  public getWardByDistrictIds(districtIds: number[], isHideExistWard: boolean = false, arrCols: string[] = []): Promise<ResponseModel> {
    let obj = new Object();
    obj['ids'] = districtIds;
    obj['isHideExistWard'] = isHideExistWard;
    obj['cols'] = arrCols.join(",");

    return super.postCustomApi("getWardByDistrictIds", obj);
  }

  async getWardByDistrictIdsAsync(districtIds: number[], isHideExistWard: boolean = false, arrCols: string[] = []): Promise<Ward[]> {
    const res = await this.getWardByDistrictIds(districtIds, isHideExistWard, arrCols);
    if (res.isSuccess) {
      let data = res.data as Ward[];
      data = SortUtil.sortAlphanumericalPram(data, "name");
      return data;
    }
    return null;
  }

  getWardByName(name: string, districtId: number = null) {
    let params = new HttpParams();
    params = params.append("name", name);

    if (districtId) {
      params = params.append("districtId", districtId.toString());
    }

    return super.getCustomApi("getWardByName", params);
  }

  public updateWard(wardId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("wardId", wardId);
    return super.getCustomApi("UpdateWard", params);
  }
}
