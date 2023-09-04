import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Constant } from '../../infrastructure/constant';
import { ResponseModel } from '../../models/entity/response.model';
import { LoginModel } from '../../models/entity/login.model';
import { MsgService } from '../local/msg.service';
import { StorageService } from '../local/storage.service';
import * as jwt_decode from 'jwt-decode';
import { ForgotPasswordModel } from '../../models/entity/forgotPassword.model';
import { ChangePasswordModel } from '../../models/entity/changePassword.model';
import { User } from '../../models/entity/user.model';
import { GeneralService } from './general.service';
import { SortUtil } from '../../infrastructure/sort.util';
import { SelectModel } from '../../models/entity/Selected.model';
import { FilterViewModel } from '../../models/viewModel/filter.viewModel';
import { SelectItem } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends GeneralService {

  constructor(
    protected messageService: MsgService,
    protected httpClient: HttpClient,
    private router: Router,
    private storageService: StorageService,
  ) {
    super(messageService, httpClient, environment.opapiUrl, 'Account');
  }

  async login(model: LoginModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('SignIn', model);
    if (res.isSuccess) {
      this.storageService.set(Constant.auths.expires, res.data.expires);
      this.storageService.set(Constant.auths.isLoginIn, 'true');
      this.storageService.set(Constant.auths.token, res.data.token);
      this.storageService.set(Constant.auths.userId, res.data.userId);
      this.storageService.set(Constant.auths.userName, res.data.userName);
      this.storageService.set(Constant.auths.fullName, res.data.userFullName);
    }
    return res;
  }

  logout(): void {
    this.storageService.removeAll();
    this.router.navigate(['/login']);
  }

  isLogged(): boolean {
    const token = this.storageService.get(Constant.auths.token);
    if (token) {
      const jwt = jwt_decode(token);
      if (jwt.exp * 1000 > new Date().valueOf()) {
        return true;
      }
    }
    return false;
  }

  async forgotPassword(model: ForgotPasswordModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('ForgotPassword', model);
    return res;
  }

  async getInfoUserById(id: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('id', id);
    const res = await this.getCustomApi('GetInfoUserById', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async changePassWord(model: ChangePasswordModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('ChangePassWord', model);
    return res;
  }

  async getUsers(model: any): Promise<ResponseModel> {
    const res = await this.postCustomApi('GetUsers', model);
    return res;
  }

  async getUsersBySearchCode(model: any): Promise<ResponseModel> {
    const res = await this.postCustomApi('GetUsersBySearchCode', model);
    return res;
  }

  async updateUser(model: User): Promise<ResponseModel> {
    const res = await this.postCustomApi('Update', model);
    return res;
  }

  async getEmpByCurrentHub(hubId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('hubId', hubId);
    const res = await this.getCustomApi('GetEmpByCurrentHub', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getEmpByCurrentHubAsync(hubId: any): Promise<any[]> {
    const res = await this.getEmpByCurrentHub(hubId);
    if (res.isSuccess) {
      let data = res.data as any[];
      data = SortUtil.sortAlphanumericalPram(data, 'name');
      return data;
    }
    return null;
  }

  public async getSelectModelEmpByCurrentHubAsync(hubId: any): Promise<SelectModel[]> {
    const res = await this.getEmpByCurrentHubAsync(hubId);
    const data: SelectModel[] = [];
    data.push({ label: '-- Chọn nhân viên --', value: null });
    if (res) {
      res.forEach(element => {
        data.push({ label: `${element.code} - ${element.fullName}`, value: element.id, data: element });
      });
      return data;
    } else {
      return null;
    }
  }

  async search(model: FilterViewModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('search', model);
    return res;
  }

  async searchCodeName(model: FilterViewModel): Promise<ResponseModel> {
    const res = await this.postCustomApi('SearchCodeName', model);
    return res;
  }

  async getUserByTypeUser(): Promise<ResponseModel> {
    let params = new HttpParams();
    const res = await this.getCustomApi('GetUserByTypeUser', params);
    return res;
  }

  async getUserByTypeUserAsync(): Promise<SelectItem[]> {
    const res = await this.getUserByTypeUser();
    if (res.isSuccess) {
      const selectModel = [{ label: '-- Chọn dữ liệu --', data: null, value: null }];
      const datas = res.data as any[];
      datas.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.fullName}`,
          data: element,
          value: element.id
        });
      });
      return selectModel;
    }
  }

  async getUserById(userId: any): Promise<ResponseModel> {
    let params = new HttpParams();
    params = params.append('userId', userId);
    const res = await this.getCustomApi('GetUserById', params);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }

  async getUsersBySearch(viewModel: any): Promise<ResponseModel> {
    const res = await this.postCustomApi('GetUsersBySearch', viewModel);
    if (!this.isValidResponse(res)) { return; }
    return res;
  }
}
