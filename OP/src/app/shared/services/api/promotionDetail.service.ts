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

export class PromotionDetailService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PromotionDetail');
  }

  async searchPromotionDetail(viewModel: any): Promise<ResponseModel> {
    const res = await this.postCustomApi('SearchPromotionDetail', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async updatePromotionDetail(model: PromotionDetail): Promise<ResponseModel> {
    const res = await this.postCustomApi('Update', model);
    return res;
  }

  async getListPromotionDetailByPromotionId(promotionId :any):Promise<PromotionDetail[]>{
    let paramas = new HttpParams();
    paramas = paramas.append("promotionId",promotionId);
    const res = await super.getCustomApi("GetListPromotionDetailByPromotionId",paramas);
    if(!this.isValidResponse(res)) return;
    return res.data;
  }
}
