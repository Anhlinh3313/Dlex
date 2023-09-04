import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SortUtil } from '../../infrastructure/sort.util';
import { ResponseModel } from '../../models/entity/response.model';
import { SelectModel } from '../../models/entity/Selected.model';
import { FilterViewModel } from '../../models/viewModel/filter.viewModel';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class CountryService extends GeneralService {
    constructor(
      protected messageService: MsgService,
      protected httpClient: HttpClient,
    ) {
      super(messageService, httpClient, environment.opapiUrl, 'Country');
    }
    //
    async getCountry(viewModel: any): Promise<ResponseModel> {
      let param = new HttpParams();
      param = param.append('pageNumber', viewModel.pageNumber);
      param = param.append('pageSize', viewModel.pageSize);
      const res = await this.getCustomApi('GetAll', param);
      if (!this.isValidResponse(res)) { return; }
      return res;
    }

    async search(model: FilterViewModel): Promise<ResponseModel> {
      const res = await this.postCustomApi('search', model);
      return res;
    }

    public updateCountry(countryId: any): Promise<ResponseModel> {
      let params = new HttpParams();
      params = params.append("countryId", countryId);
      return super.getCustomApi("UpdateCountry", params);
    }

    async getCountryAsync(): Promise<SelectModel[]> {
      const res = await this.getAll();
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

    async getListCountrys(viewModel: any): Promise<ResponseModel> {
      const res = await this.postCustomApi('GetListCountrys', viewModel);
      if (!this.isValidResponse(res)) { return; }
      return res;
    }
}
