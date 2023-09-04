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
export class DepartmentService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Department');
   }

   async getDepartments(): Promise<ResponseModel> {
    const params = new HttpParams();
    const res = await this.getCustomApi('GetAll', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
   }

   async getDepartmentAsync(): Promise<SelectModel[]> {
    const res = await this.getDepartments();
    if (res.isSuccess) {
      let selectModel = [];
      let datas = res.data as any[];

      datas = SortUtil.sortAlphanumerical(datas, 1, "code");
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: "-- Chọn dữ liệu --", data: null, value: null });
      return selectModel;
    }
  }
}