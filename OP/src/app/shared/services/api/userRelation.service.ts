import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { Ward } from '../../models/entity/ward.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class UserRelationService extends GeneralService {
    constructor(
      protected messageService: MsgService,
      protected httpClient: HttpClient,
    ) {
      super(messageService, httpClient, environment.opapiUrl, 'UserRelation');
    }
    //
    async getUserRelation(viewModel: any): Promise<ResponseModel> {
      const param = new HttpParams();
      const res = await this.postCustomApi('GetUserRelation', viewModel);
      if (!this.isValidResponse(res)) { return; }
      return res;
    }

    async getUserByUserRelationId(userId: any ): Promise<ResponseModel> {
        let params = new HttpParams();
        params = params.append('userId', userId);
        const res = await this.getCustomApi('GetUserByUserRelationId', params);
        if (!this.isValidResponse(res)) { return; }
        return res;
      }
    
      async getUserByUserRelationIdAsync(userId: any): Promise<any[]> {
        const res = await this.getUserByUserRelationId(userId);
        if (res.isSuccess) {
          let data = res.data as any[];
          data = SortUtil.sortAlphanumericalPram(data,"name");
          return data;
        }
        return null;
      }
    
      public async getSelectModelUserByUserRelationIdAsync(userId: any): Promise<SelectModel[]> {
        const res = await this.getUserByUserRelationIdAsync(userId);
        const data: SelectModel[] = [];
        data.push({ label: "-- Chọn nhân viên cấp dưới --", value: null });
        if (res) {
          res.forEach(element => {
            data.push({ label: `${element.code} - ${element.name}`, value: element.id, data: element });
          });
          return data;
        } else {
          return null;
        }
      }

      async createUserRelation(viewModel: any): Promise<ResponseModel> {
        const param = new HttpParams();
        const res = await this.postCustomApi('CreateUserRelation', viewModel);
        return res;
      }
  

}