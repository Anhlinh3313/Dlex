import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class HubRouteService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'HubRoute');
   }

  public getDatasFromHub(hubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("hubId", hubId);
    return super.getCustomApi("getDatasFromHub", params);
  }

  public GetHubRouteByWardIds(...params): Promise<ResponseModel> {
    let model = { Ids: params[0], HubId: params[1] };

    return super.postCustomApi("GetHubRouteByWardIds", model);
  }
  
  async getHubRouteByWardIdsAsync(...params): Promise<any[]> {
    const res = await this.GetHubRouteByWardIds(...params);
    if (res.isSuccess) {
      let data = res.data as any[];
      data = SortUtil.sortAlphanumericalPram(data, "name");
      return data;
    }
    return null;
  }

  public saveChangeHubRoute(hubId: any, wardIds: number[]): Promise<ResponseModel> {
    let obj = new Object();
    obj["hubId"] = hubId;
    obj["wardIds"] = wardIds;

    return super.postCustomApi("saveChangeHubRoute", obj);
  }

}
