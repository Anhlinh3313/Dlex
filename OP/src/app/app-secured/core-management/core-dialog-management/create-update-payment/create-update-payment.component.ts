import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PaymentType } from 'src/app/shared/models/entity/paymentType.model';
import { PaymentTypeService } from 'src/app/shared/services/api/paymentType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-payment',
  templateUrl: './create-update-payment.component.html',
  styleUrls: ['./create-update-payment.component.scss']
})
export class CreateUpdatePaymentComponent extends BaseComponent implements OnInit {
  edit = false;
  payment: PaymentType = new PaymentType();

  constructor(
    public config: DynamicDialogConfig,
    protected paymentTypeService: PaymentTypeService,
    public ref: DynamicDialogRef,
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
    if (this.config.data) {
      this.payment = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updatePayment();
    } else {
      this.createPayment();
    }
  }

  async updatePayment() {
    if (!this.isValidData()) { return; }
    this.payment.isEnabled = true;
    this.payment.code = this.payment.code.trim();
    this.payment.name = this.payment.name.trim();
    let res = await this.paymentTypeService.update(this.payment)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createPayment() {
    if (!this.isValidData()) { return; }
    this.payment.code = this.payment.code.trim();
    this.payment.name = this.payment.name.trim();
    let res = await this.paymentTypeService.create(this.payment)
    if (res.isSuccess) {
      this.msgService.success('Tạo hình thức thanh toán thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo hình thức thanh toán không thành công');
    }
  }

  isValidData(): boolean {

    if (!this.payment.code) {
      this.msgService.error('Vui lòng nhập mã hình thức thanh toán');
      return false;
    }

    if (!this.payment.name) {
      this.msgService.error('Vui lòng nhập tên hình thức thanh toán');
      return false;
    }

    return true;
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }
}
