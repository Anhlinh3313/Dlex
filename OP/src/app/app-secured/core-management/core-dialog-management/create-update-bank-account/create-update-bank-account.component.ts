import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { AccountBank } from 'src/app/shared/models/entity/accountBank.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { AccountBankService } from 'src/app/shared/services/api/accountBank.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-bank-account',
  templateUrl: './create-update-bank-account.component.html',
  styleUrls: ['./create-update-bank-account.component.scss']
})
export class CreateUpdateBankAccountComponent extends BaseComponent implements OnInit {

  edit = false;
  accountBank: AccountBank = new AccountBank();
  bank: SelectModel[] = [];
  selectedBank: SelectModel;
  branch: SelectModel[] = [];
  selectedBranch: SelectModel;

  constructor(
    protected accountBankService: AccountBankService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    await this.loadBank();
    if (this.config.data) {
      this.accountBank = this.config.data;
      this.edit = true;
      const findBank = this.bank.find(f => f.value === this.accountBank.bankId);
      if (findBank) {
        this.selectedBank = findBank;
        await this.onChangeBank();
        const findBranch = this.branch.find(f => f.value === this.accountBank.branchId);
        if (findBranch) {
          this.selectedBranch = findBranch;
        }
      }
      // const findbranch = this.bank.find(f => f.value === this.accountBank.branchId);
      // if (findbranch) {
      //   this.selectedBranch = findbranch;
      // }
    }
  }

  async loadBank() {
    this.bank = await this.accountBankService.getBankAllAsync();
  }

  async onChangeBank() {
    let bankId = this.selectedBank.value;
    this.branch = await this.accountBankService.getBranchByAsync(bankId);
    this.selectedBranch = null;
  }

  onChangeBranch() { }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updateAccountBank();
    } else {
      this.createAccountBank();
    }
  }

  async updateAccountBank(){
    if (!this.isValidData()) { return; }
    this.accountBank.code = this.accountBank.code.trim()
    this.accountBank.branchId = this.selectedBranch.value;
    this.accountBank.name = this.accountBank.code.trim();
    this.accountBank.isEnabled = true;
    let res = await this.accountBankService.update(this.accountBank)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createAccountBank(){
    if (!this.isValidData()) { return; }
    this.accountBank.code = this.accountBank.code.trim();
    this.accountBank.branchId = this.selectedBranch.value;
    this.accountBank.name = this.accountBank.code.trim();
    let res = await this.accountBankService.create(this.accountBank)
    if (res.isSuccess) {
      this.msgService.success('Tạo tài khoản ngân hàng thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo tài khoản ngân hàng không thành công');
    }
  }

  isValidData(): boolean {

    if (!this.accountBank.code) {
      this.msgService.error('Vui lòng nhập mã tài khoản ngân hàng');
      return false;
    }

    if (!this.selectedBank) {
      this.msgService.error('Vui lòng chọn ngân hàng');
      return false;
    }

    
    if (!this.selectedBranch) {
      this.msgService.error('Vui lòng chọn chi nhánh');
      return false;
    }

    return true;
  }
}
