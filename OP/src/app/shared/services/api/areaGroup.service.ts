import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MsgService } from '../local/msg.service';
import { StorageService } from '../local/storage.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class AreaGroupService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'areaGroup');
  }
}
