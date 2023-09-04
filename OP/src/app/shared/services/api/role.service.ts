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

export class RoleService extends GeneralService {
    constructor(
      protected messageService: MsgService,
      protected httpClient: HttpClient,
    ) {
      super(messageService, httpClient, environment.opapiUrl, 'Role');
    }
    //
    async getRole(model: any): Promise<ResponseModel> {
      const res = await this.postCustomApi('GetRoles', model);
      if (!this.isValidResponse(res)) { return; }
      return res;
    }

    async getRoles(): Promise<ResponseModel> {
      const params = new HttpParams();
      const res = await this.getCustomApi('GetAll', params);
      if (!this.isValidResponse(res)) { return; }
      return res;
     }

    async getRoleAsync(): Promise<SelectModel[]> {
      const res = await this.getRoles();
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
        return selectModel;
      }
    }

    async getRolesMultiSelectAsync(): Promise<SelectModel[]> {
      const res = await this.getRoles();
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
        return selectModel;
      }
    }

}
