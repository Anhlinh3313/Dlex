import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { PromotionDetail } from '../../models/entity/promotionDetail.model';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class PromotionService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Promotion');
  }

  async getListPromotion(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListPromotion', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public getAllPromotionType(): Promise<ResponseModel> {
    let params = new HttpParams();
    return super.getCustomApi('GetAllPromotionType',  params);
  }

  async getAllPromotionTypeAsync(): Promise<SelectModel[]> {
    const res = await this.getAllPromotionType();
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

  async getAllPromotionAsync(): Promise<SelectModel[]> {
    const res = await this.getAll();
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

  async deleteById(id: any ): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('promotionId', id);
    const res = await this.getCustomApi('Delete', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }
}
