import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GMapHelper } from '../../infrastructure/gmap.helper';
import { SortUtil } from '../../infrastructure/sort.util';
import { Hub } from '../../models/entity/Hub.model';
import { InfoLocation } from '../../models/entity/infoLocation.model';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class HubService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Hub');
   }

   async getCenterHub(): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.getCustomApi('GetCenterHub', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async getPoHub(): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.getCustomApi('GetPoHub', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async getStationHub(): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.getCustomApi('GetStationHub', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async getCenterHubAsync(): Promise<SelectModel[]> {
    const res = await this.getCenterHub();
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];
      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }

  async getPoHubAsync(): Promise<SelectModel[]> {
    const res = await this.getPoHub();
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];

      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }

  async getStationHubAsync(): Promise<SelectModel[]> {
    const res = await this.getStationHub();
    if (res.isSuccess) {
      const selectModel = [];
      let datas = res.data as any[];

      datas = SortUtil.sortAlphanumerical(datas, 1, 'code');
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu --', data: null, value: null });
      return selectModel;
    }
  }

  async getCenterHubs(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('GetCenterHubs', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getPoHubs(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('GetPoHubs', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async GetStationHubs(viewModel: any): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.postCustomApi('GetStationHubs', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public getPoHubByCenterId(centerId: any, arrCols: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('centerId', centerId);
    params = params.append('arrCols', arrCols);
    return super.getCustomApi('getPoHubByCenterId', params);
  }

  async getPoHubByCenterIdAsync(centerId: any, arrCols: string[] = []): Promise<Hub[]> {
    const res = await this.getPoHubByCenterId(centerId, arrCols);
    if (res.isSuccess) {
      let data = res.data as Hub[];
      data = SortUtil.sortAlphanumericalPram(data, 'name');
      return data;
    }
    return null;
  }

  public async getSelectModelPoHubByCenterIdbAsync(centerId: any, arrCols: string[] = []): Promise<SelectModel[]> {
    const res = await this.getPoHubByCenterIdAsync(centerId, arrCols);
    const data: SelectModel[] = [];
    data.push({ label: '-- Chọn dữ liệu --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: `${element.code} - ${element.name}`, value: element.id, data: element });
      });
      return data;
    } else {
      return null;
    }
  }

  public getStationHubByPoId(poId: any, arrCols: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('poId', poId);
    params = params.append('arrCols', arrCols);
    return super.getCustomApi('getStationHubByPoId',  params);
  }

  async getStationHubByPoIdAsync(poId: any, arrCols: string[] = []): Promise<Hub[]> {
    const res = await this.getStationHubByPoId(poId, arrCols);
    if (res.isSuccess) {
      let data = res.data as Hub[];
      data = SortUtil.sortAlphanumericalPram(data, 'name');
      return data;
    }
    return null;
  }

  public async getSelectModelAsync(poId: any, arrCols: string[] = []): Promise<SelectModel[]> {
    const res = await this.getStationHubByPoIdAsync(poId, arrCols);
    const data: SelectModel[] = [];
    data.push({  label: '-- Chọn dữ liệu --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: element.name, value: element.id, data: element });
      });
      return data;
    } else {
      return null;
    }
  }

  public async getListHubFromHubId(hubId: any): Promise<Hub[]> {
    let params = new HttpParams();
    params = params.append('hubId', hubId);
    const res = await this.getCustomApi('GetListHubFromHubId', params);
    if (res.isSuccess) {
      let data = res.data as Hub[];
      data = SortUtil.sortAlphanumericalPram(data, 'name');
      return data;
    }
    return [];
  }

  public async getListHubFromHubIdSelectModelAsync(hubId: any): Promise<SelectModel[]> {
    const res = await this.getListHubFromHubId(hubId);
    const data: SelectModel[] = [];
    data.push({  label: '-- Chọn dữ liệu --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: element.name, value: element.id, data: element });
      });
      return data;
    } else {
      return [];
    }
  }

  public async getListHubFromHubIdUserSelectModelAsync(hubId: any): Promise<SelectModel[]> {
    const res = await this.getListHubFromHubId(hubId);
    const data: SelectModel[] = [];
    data.push({  label: '-- Chọn đơn vị làm việc --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: element.name, value: element.id, data: element });
      });
      return data;
    } else {
      return [];
    }
  }

  public updateCenterHub(centerHubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("centerHubId", centerHubId);
    return super.getCustomApi("UpdateCenterHub", params);
  }

  public updatePoHub(poHubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("poHubId", poHubId);
    return super.getCustomApi("UpdatePoHub", params);
  }

  public updateStationHub(stationHubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("stationHubId", stationHubId);
    return super.getCustomApi("UpdateStationHub", params);
  }

  async getInfoLocation(provinceName?: any, districtName?: any, wardName?: any, provinceId?: any, districtId?: any, wardId?: any, countryId?: any): Promise<InfoLocation> {
    let params = new HttpParams();
    params = params.append('provinceName', provinceName);
    params = params.append('districtName', districtName);
    params = params.append('wardName', wardName);
    params = params.append('provinceId', provinceId);
    params = params.append('districtId', districtId);
    params = params.append('wardId', wardId);
    params = params.append('countryId', countryId);
    const res = await this.getCustomApi('GetInfoLocation', params);
    if (!this.isValidResponse(res)) { return; }
    const data = res.data as InfoLocation;
    return data;
  }

  async getLocationByGeocoderResult(geocoderResult: google.maps.GeocoderResult[]) {
    let results;
    const resultIndex1 = geocoderResult.findIndex(f => f.types[2] === 'sublocality_level_1');
    const resultIndex2 = geocoderResult.findIndex(f => f.types[0] === 'administrative_area_level_3');
    if (resultIndex1 > resultIndex2) {
      results = geocoderResult.find(f => f.types[2] === 'sublocality_level_1');
      if (!results) {
        results = geocoderResult.find(f => f.types[0] === 'administrative_area_level_3');
      }
    } else {
      results = geocoderResult.find(f => f.types[0] === 'administrative_area_level_3');
      if (!results) {
        results = geocoderResult.find(f => f.types[2] === 'sublocality_level_1');
      }
    }
    const infoLocation = new InfoLocation();
    if (results) {
      results.address_components.map(element => {
        //
        if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
          infoLocation.provinceName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1) {
          infoLocation.wardName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
          infoLocation.wardName = element.long_name;
        }
      });
    }
    const locationInfo = await this.getInfoLocation(infoLocation.provinceName, infoLocation.districtName, infoLocation.wardName);
    return locationInfo;
  }

  async getHubByUserId(userId?: any): Promise<InfoLocation> {
    let params = new HttpParams();
    params = params.append('userId', userId);
    const res = await this.getCustomApi('GetHubByUserId', params);
    if (!this.isValidResponse(res)) { return; }
    const data = res.data as InfoLocation;
    return data;
  }

  async getHubByProvinceDistrictWard(provinceId: any, districtId: any, wardId: any): Promise<any> {
    let params = new HttpParams();
    params = params.append('provinceId', provinceId);
    params = params.append('districtId', districtId);
    params = params.append('wardId', wardId);
    const res = await this.getCustomApi('GetHubByProvinceDistrictWard', params);
    if (!this.isValidResponse(res)) { return; }
    const data = res.data;
    return data;
  }

  
  public async getHubByProvinceDistrictWardSelectModelAsync(provinceId: any, districtId: any, wardId: any): Promise<SelectModel[]> {
    const res = await this.getHubByProvinceDistrictWard(provinceId, districtId, wardId);
    const data: SelectModel[] = [];
    data.push({  label: '-- Chọn dữ liệu --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: `${element.code} - ${element.name}`, value: element.id, data: element });
      });
      return data;
    } else {
      return [];
    }
  }
}
