import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class HubRoutingService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'HubRouting');
   }

  public getHubRoutingByPoHubId(stationHubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("stationHubId", stationHubId);
    return super.getCustomApi("GetHubRoutingByPoHubId", params);
  }

  public getDatasFromHub(stationHubId: any, hubRoutingId: any, isTruckDelivery: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("stationHubId", stationHubId);
    params = params.append("hubRoutingId", hubRoutingId);
    params = params.append("isTruckDelivery", isTruckDelivery);
    return super.getCustomApi("GetDatasFromHub", params);
  }

  create(obj: Object): Promise<ResponseModel> {
    return super.postCustomApi("create", obj);
  }

  update(obj): Promise<ResponseModel> {
    return super.postCustomApi("update", obj);
  }

  public getWardByHubId(hubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("hubId", hubId);
    return super.getCustomApi("GetWardByHubId", params);
  }

  public getWardIdsByHubId(hubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("hubId", hubId);
    return super.getCustomApi("GetWardIdsByHubId", params);
  }
}
