import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class PageService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Page');
  }

  async getPageByModuleId(moduleId: any): Promise<ResponseModel> {
    let param = new HttpParams();
    param = param.append('id', moduleId);
    param = param.append('isDisplayAll', null);
    const res = await this.getCustomApi('GetPageByModuleId', param);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  getMenuByModuleId(id: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("id", id);

    return super.getCustomApi("getMenuByModuleId", params);
  }

  async getMenuByModuleIdAsync(id: any): Promise<any> {
    const res = await this.getMenuByModuleId(id);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }
}
