import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Area } from '../../models/entity/area.model';
import { MsgService } from '../local/msg.service';
import { StorageService } from '../local/storage.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root'
})
export class AreaService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
    private router: Router,
    private storageService: StorageService,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'area');
  }

  updateAreaDistricts(model: any) {
    return super.postCustomApi("UpdateAreaDistricts", model);
  }

  async updateAreaDistrictsAsync(model: any): Promise<any> {
    const res = await this.updateAreaDistricts(model);
    if (!this.isValidResponse(res)) return null;
    const data = res.data;
    return data;
  }

  deleteArea(model: Area) {
    return super.postCustomApi("DeleteArea", model);
  }

  async deleteAreaAsync(model: Area): Promise<Area> {
    const res = await this.deleteArea(model);
    if (!this.isValidResponse(res)) return;
    const data = res.data as Area;
    return data;
  }

}
