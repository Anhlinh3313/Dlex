import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class ModulePageService extends GeneralService {
    constructor(
      protected messageService: MsgService,
      protected httpClient: HttpClient,
    ) {
      super(messageService, httpClient, environment.opapiUrl, 'ModulePage');
    }
}
