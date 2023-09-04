import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SelectItem } from 'primeng/api';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { StorageService } from '../local/storage.service';
import { BaseService } from './base.service';

@Injectable({
    providedIn: 'root'
  })

export class CompanyService extends BaseService{
    constructor(
      protected messageService: MsgService,
      protected httpClient: HttpClient,
    ) {
      super(messageService, httpClient, environment.opapiUrl, 'Company');
    }
    //
    async getCompany(): Promise<ResponseModel> {
        const params = new HttpParams();
        const res = await this.getCustomApi('GetCompany', params);
        if (!this.isValidResponse(res)) { return; }
        return res;
    }
}
