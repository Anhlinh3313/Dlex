import { Component, OnInit } from '@angular/core';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { SelectModel } from '../../../shared/models/entity/Selected.model';
import { AuthService } from '../../../../app/shared/services/api/auth.service';
import { MsgService } from '../../../../app/shared/services/local/msg.service';
import { StorageService } from '../../../shared/services/local/storage.service';
import { Constant } from '../../../shared/infrastructure/constant';
import { IconInputModel } from '../../../../app/shared/models/entity/icon.model';
import { environment } from '../../../../environments/environment';
import { ChangePasswordModel } from 'src/app/shared/models/entity/changePassword.model';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  //
  hubs: SelectModel[] = [];
  selectedHub: any;
  userId: number;
  changePass: string;
  changePasswordModel: ChangePasswordModel = new ChangePasswordModel();
  inputElementNewPassword: IconInputModel = new IconInputModel();
  inputElementConfirmPassword: IconInputModel = new IconInputModel();
  inputElementConfirmChangePass: IconInputModel = new IconInputModel();
  eyeShow = environment.eyeShow;
  eyeHide = environment.eyeHide;
  //
  constructor(
    private storageService: StorageService,
    protected msgService: MsgService,
    public ref: DynamicDialogRef,
    //
    private authService: AuthService,
  ) {
  }

  ngOnInit(): void {
    this.intData();
  }

  intData(): void {
    this.loadUserLogin();
    this.inputElementNewPassword.src = this.eyeHide;
    this.inputElementNewPassword.type = 'password';
    this.inputElementConfirmPassword.src = this.eyeHide;
    this.inputElementConfirmPassword.type = 'password';
    this.inputElementConfirmChangePass.src = this.eyeHide;
    this.inputElementConfirmChangePass.type = 'password';
  }

  closeDialog(): void {
    if (this.ref) {
      this.ref.close();
    }
  }

  async loadUserLogin(): Promise<any> {
    this.userId = this.storageService.get(Constant.auths.userId);
  }

  logout(): void {
    this.authService.logout();
  }

  async chanPassword(): Promise<any> {
    if (!this.isValidData()) { return; }
    this.changePasswordModel.userId = this.userId;
    const res = await this.authService.changePassWord(this.changePasswordModel);
    if (res.isSuccess) {
      this.msgService.success('cập nhật mật khẩu thành công');
      this.closeDialog();
      this.logout();
    }
    else {
      this.msgService.error(res.message);
    }
  }

  isValidData(): boolean {
    const patternPassword = /\s/g;
    const strongPassword = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])");
    
    if (this.changePasswordModel.currentPassWord == this.changePasswordModel.newPassWord) {
      this.msgService.error('Vui lòng mật khẩu không được trùng với mật khẩu củ');
      return false;
    }

    if (!this.changePasswordModel.currentPassWord) {
      this.msgService.error('Vui lòng mật khẩu củ');
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

  onClickConfirmChangePass() {
    if (this.inputElementConfirmChangePass.src === this.eyeShow) {
      this.inputElementConfirmChangePass.src = this.eyeHide;
      this.inputElementConfirmChangePass.type = 'password';
    } else if (this.inputElementConfirmChangePass.src === this.eyeHide) {
      this.inputElementConfirmChangePass.src = this.eyeShow;
      this.inputElementConfirmChangePass.type = 'text';
    }
  }
}
