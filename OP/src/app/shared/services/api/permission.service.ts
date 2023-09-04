import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/entity/response.model';
import { RolePage } from '../../models/entity/rolePage.model';
import { MsgService } from '../local/msg.service';
import { GeneralService } from './general.service';

@Injectable({
    providedIn: 'root'
  })

export class PermissionService extends GeneralService {
  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Permission');
  }
  //
  async getPermissionByRoleId(roleId: any): Promise<ResponseModel> {
    let param = new HttpParams();
    param = param.append('roleId', roleId);
    const res = await this.getCustomApi('GetByRoleId', param);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getByRolePageByPageId(roleId: any, pageId: any): Promise<ResponseModel> {
    let param = new HttpParams();
    param = param.append('roleId', roleId);
    param = param.append('pageId', pageId);
    const res = await this.getCustomApi('GetByRolePageByPageId', param);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async updatePermission(rolePages: RolePage[]): Promise<ResponseModel> {
    const res = await this.postCustomApi('updatePermission', rolePages);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public async checkPermissionDetail(aliasPath: any, moduleId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('aliasPath', aliasPath);
    params = params.append('moduleId', moduleId);
    const res = await this.getCustomApi('CheckPermissionDetail', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  public async getAllPermissionByRoleId(roleId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('roleId', roleId);
    const res = await this.getCustomApi('GetAllPermissionByRoleId', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

}
