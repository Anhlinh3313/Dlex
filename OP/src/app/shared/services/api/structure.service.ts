import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class StructureService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Structure');
  }

  async getListStructure(viewModel: any): Promise<ResponseModel> {
    const param = new HttpParams();
    const res = await this.postCustomApi('GetListStructure', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }
}
