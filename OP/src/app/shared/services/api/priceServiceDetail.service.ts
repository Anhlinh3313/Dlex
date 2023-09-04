import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PriceServiceDetail } from '../../models/entity/priceServiceDetail.model';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class PriceServiceDetailService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'PriceServiceDetail');
  }

  public getByPriceServiceId(areaGroupId: number, priceServiceId: number): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('areaGroupId', areaGroupId + '');
    params = params.append('priceServiceId', priceServiceId + '');
    return super.getCustomApi('GetByPriceServiceId', params);
  }

  getByPriceServiceDetail(model: any): Promise<ResponseModel> {
    return super.postCustomApi('GetByPriceServiceDetail', model);
  }
  
  async getByPriceServiceDetailAsync(model: any): Promise<any> {
    const res = await this.getByPriceServiceDetail(model);
    if (!this.isValidResponse(res)) { return; }
    const data = res.data;
    return data;
  }

  async getByPriceServiceIdAsync(priceSerivceId: any, areaGroupId: any): Promise<any> {
    const res = await this.getByPriceServiceId(priceSerivceId, areaGroupId);
    if (!this.isValidResponse(res)) return;
    const data = res.data as any;
    return data;
  }

  creatPriceServices(model: PriceServiceDetail) {
    return super.postCustomApi("create", model);
  }

  async creatPriceServicesAsync(model: PriceServiceDetail): Promise<PriceServiceDetail> {
    const res = await this.creatPriceServices(model);
    if (!this.isValidResponse(res)) return;
    const data = res.data as PriceServiceDetail;
    return data;
  }

  updatePriceServices(model: PriceServiceDetail[]) {
    return super.postCustomApi("UpdatePriceServices", model);
  }

  async updatePriceServicesAsync(model: PriceServiceDetail[]): Promise<PriceServiceDetail[]> {
    const res = await this.updatePriceServices(model);
    if (!this.isValidResponse(res)) return;
    const data = res.data as PriceServiceDetail[];
    return data;
  }

}
