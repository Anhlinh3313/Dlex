import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService } from './base.service';
import { MsgService } from '../local/msg.service';
import { StorageService } from '../local/storage.service';
import { SelectItem } from 'primeng/api';
import { ResponseModel } from '../../models/entity/response.model';

@Injectable()
export class GeneralService extends BaseService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
    @Inject(String) protected urlName: string,
    @Inject(String) protected apiName: string
  ) {
    super(messageService, httpClient, urlName, apiName);
  }

  async getById(id: any, arrCols: string[] = []): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('id', id);

    if (arrCols.length > 0) {
      params = params.append('cols', arrCols.join(','));
    }

    const res = await this.getCustomApi('Get', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }


  async getAll(arrCols: string[] = [], pageSize: number = 0, pageNumber: number = 0): Promise<ResponseModel> {
    let params = new HttpParams();

    if (arrCols.length > 0) {
      params = params.append('cols', arrCols.join(','));
    }

    if (pageSize) {
      params = params.append('pageSize', pageSize + '');
    }

    if (pageNumber) {
      params = params.append('pageNumber', pageNumber + '');
    }

    const res = await this.getCustomApi('GetAll', params);
    return res;
  }

  async  create(model: object): Promise<ResponseModel> {
    const res = await this.postCustomApi('Create', model);
    return res;
  }

  async update(model: object): Promise<ResponseModel> {
    const res = await this.postCustomApi('Update', model);
    return res;
  }

  async delete(model: object): Promise<ResponseModel> {
    const res = await this.postCustomApi('Delete', model);
    return res;
  }

  async getAllSelectModelAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();
    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn dữ liệu --', data: null, value: null }];
      const datas = res.data as any[];
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getAllMultiSelectModelAsync(): Promise<SelectItem[]> {
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

  async getAllSelectFullNameModelAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();
    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn dữ liệu --', data: null, value: null }];
      const datas = res.data as any[];
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.fullName}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getAllSelectModelProjectAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();

    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn dự án --', data: null, value: null }];
      const datas = res.data as any[];

      datas.forEach(element => {
        selectModel.push({

          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getAllSelectModelRoleAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();

    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn quyền --', data: null, value: null }];
      const datas = res.data as any[];

      datas.forEach(element => {
        selectModel.push({

          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getAllSelectModelLegalAsync(): Promise<SelectItem[]> {
    const res = await this.getAll();

    if (res.isSuccess) {
      const selectModel = [];
      const datas = res.data as any[];

      datas.forEach(element => {
        selectModel.push({
          label: `${element.legalCode} - ${element.companyName}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getAllAsync(
    arrCols: string[] = [],
    pageSize: number = 0,
    pageNumber: number = 0
  ): Promise<any> {
    return await this.getAll(arrCols, pageSize, pageNumber);
  }

  async createAsync(model: Object): Promise<ResponseModel> {
    return await this.create(model);
  }

  async updateAsync(model: Object): Promise<ResponseModel> {
    return await this.update(model);
  }

  async deleteAsync(model: Object): Promise<ResponseModel> {
    return await this.delete(model);
  }

}
