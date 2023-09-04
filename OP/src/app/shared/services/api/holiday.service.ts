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

export class HolidayService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Holiday');
  }

  public getHolidayByYear(year: number, pageNumber: number, pageSize: number): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append("year", year+'');
    params = params.append("pageNumber", pageNumber+'');
    params = params.append("pageSize", pageSize+'');
    return super.getCustomApi('GetHolidayByYear',  params);
  }
  
}
