import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { promise } from 'protractor';
import { environment } from 'src/environments/environment';
import { ChangePasswordComponent } from '../app-secured/dialog-genaral/change-password/change-password.component';
import { SelectModel } from '../shared/models/entity/Selected.model';
import { ForgotPasswordModel } from '../shared/models/entity/forgotPassword.model';
import { LoginModel } from '../shared/models/entity/login.model';
import { AuthService } from '../shared/services/api/auth.service';
import { MsgService } from '../shared/services/local/msg.service';
import { StorageService } from '../shared/services/local/storage.service';
import { Constant } from '../shared/infrastructure/constant';
import { IconInputModel } from '../shared/models/entity/icon.model';
import { ChangePasswordModel } from '../shared/models/entity/changePassword.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  //
  companys: SelectModel[] = [];
  loginModel: LoginModel = new LoginModel();
  forgotPasswordModel: ForgotPasswordModel = new ForgotPasswordModel();

  customerName = environment;
  //
  isForgetPassword = false;
  ref: DynamicDialogRef;
  selectedCompany: any;
  checkCustomer = true;
  capitalizeLetter: string;
  changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  changePass: string;
  isChangePassword: boolean = false;
  isLogin: boolean = true;
  userId: number;
  inputElementNewPassword: IconInputModel = new IconInputModel();
  inputElementConfirmPassword: IconInputModel = new IconInputModel();
  eyeShow = environment.eyeShow;
  eyeHide = environment.eyeHide;

  constructor(
    protected msgService: MsgService,
    protected dialogService: DialogService,
    private router: Router,
    private storageService: StorageService,
    private authService: AuthService,
  ) {
  }

  ngOnInit(): void {
    this.initData();
  }

  initData(): void {
    this.loadCompany();
    this.inputElementNewPassword.src = this.eyeHide;
    this.inputElementNewPassword.type = 'password';
    this.inputElementConfirmPassword.src = this.eyeHide;
    this.inputElementConfirmPassword.type = 'password';
  }

  async loadCompany(): Promise<any> {
    if (this.customerName.customerCode) {
      this.loginModel.companyCode = this.customerName.customerCode;
      this.forgotPasswordModel.companyCode = this.customerName.customerCode;
      this.checkCustomer = true;
      this.capitalizeLetter = 'text-transform: uppercase;';
    } else {
      this.checkCustomer = false;
      this.capitalizeLetter = 'text-transform: null;';
    }
  }

  async login(): Promise<any> {
    if (!this.isValidData()) { return; }
    const res = await this.authService.login(this.loginModel);
    if (res.isSuccess) {
      if (res.data.isPassWordBasic || res.data.isPassWordBasic == null) {
        this.loadUserLogin();
        this.isChangePassword = true;
        this.isLogin = false;
      }
      else {
        this.router.navigate(['/']);
      }
    }
    else {
      this.msgService.error(res.message);
    }
  }

  async sendEmail(): Promise<any> {
    if (!this.isValidDataSendEmail()) { return; }
    const ref = await this.authService.forgotPassword(this.forgotPasswordModel);
    if (ref.isSuccess) {
      this.msgService.success(ref.message);
      this.isForgetPassword = false;
      this.isLogin = true;
    }
    else {
      this.msgService.error(ref.message || "Gửi gmail không thành công");
    }
  }

  async loadUserLogin(): Promise<any> {
    this.userId = this.storageService.get(Constant.auths.userId);
  }

  async chanPassword(): Promise<any> {
    if (!this.isValidDataChanPassword()) { return; }
    this.changePasswordModel.userId = this.userId;
    this.changePasswordModel.currentPassWord = this.loginModel.password;
    const res = await this.authService.changePassWord(this.changePasswordModel);
    if (res.isSuccess) {
      this.msgService.success('cập nhật mật khẩu thành công');
      this.router.navigate(['/']);
    }
    else {
      this.msgService.error(res.message);
    }
  }

  cancelLogin(){
    this.isChangePassword = false;
    this.isLogin = true;
    this.isForgetPassword = false;
  }

  clickSendEmail(){
    this.isLogin = false;
    this.isForgetPassword = true;
    this.isChangePassword = false;
  }

  isValidDataChanPassword(): boolean {
    const patternPassword = /\s/g;
    const strongPassword = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])");

    if (this.changePasswordModel.newPassWord == this.loginModel.password) {
      this.msgService.error('Vui lòng mật khẩu không được trùng với mật khẩu củ');
      return false;
    }

    if (!this.changePasswordModel.newPassWord) {
      this.msgService.error('Vui lòng nhập mật khẩu mới');
      return false;
    }

    if (!strongPassword.test(this.changePasswordModel.newPassWord)) {
      this.msgService.error('Mật khẩu phải có đủ chữ và số');
      return false;
    }

    if (this.changePasswordModel.newPassWord.length < 8) {
      this.msgService.error('Mật khẩu phải nhiều hơn 8 ký tự.');
      return false;
    }

    if (this.changePass !== this.changePasswordModel.newPassWord) {
      this.msgService.error('Xác nhận mật khẩu không đúng');
      return false;
    }
    return true;
  }

  isValidData(): boolean {
    if (!this.loginModel.companyCode) {
      this.msgService.error('Vui lòng chọn tên công ty');
      return false;
    }
    if (!this.loginModel.username) {
      this.msgService.error('Vui lòng nhập email / số điện thoại');
      return false;
    }
    if (!this.loginModel.password) {
      this.msgService.error('Vui lòng nhập mật khẩu');
      return false;
    }
    return true;
  }

  isValidDataSendEmail(): boolean {
    if (!this.forgotPasswordModel.companyCode) {
      this.msgService.error('Vui lòng chọn tên công ty');
      return false;
    }
    if (!this.forgotPasswordModel.email) {
      this.msgService.error('Vui lòng nhập email / số điện thoại');
      return false;
    }
    return true;
  }

  onClickNewPassword() {
    if (this.inputElementNewPassword.src === this.eyeShow) {
      this.inputElementNewPassword.src = this.eyeHide;
      this.inputElementNewPassword.type = 'password';
    } else if (this.inputElementNewPassword.src === this.eyeHide) {
      this.inputElementNewPassword.src = this.eyeShow;
      this.inputElementNewPassword.type = 'text';
    }
  }

  onClickConfirmPassword() {
    if (this.inputElementConfirmPassword.src === this.eyeShow) {
      this.inputElementConfirmPassword.src = this.eyeHide;
      this.inputElementConfirmPassword.type = 'password';
    } else if (this.inputElementConfirmPassword.src === this.eyeHide) {
      this.inputElementConfirmPassword.src = this.eyeShow;
      this.inputElementConfirmPassword.type = 'text';
    }
  }

}
