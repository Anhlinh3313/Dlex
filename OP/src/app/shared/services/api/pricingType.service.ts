import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SelectItem } from 'primeng/api';
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

export class PricingTypeService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PricingType');
  }

  async getListPricingType(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListPricingType', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getAllPricingTypeAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();
    if (res.isSuccess) {
      const selectModel = [];
      const datas = res.data as any[];
      datas.forEach(element => {
        selectModel.push({
          label: `${element.name}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

}
