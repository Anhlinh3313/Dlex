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

export class PriceListSettingService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PriceListSetting');
  }

  async getListPriceListSetting(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListPriceListSetting', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }
}
