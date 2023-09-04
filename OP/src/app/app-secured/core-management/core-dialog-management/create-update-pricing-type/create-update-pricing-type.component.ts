import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PricingType } from 'src/app/shared/models/entity/pricingType.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PricingTypeService } from 'src/app/shared/services/api/pricingType.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-pricing-type',
  templateUrl: './create-update-pricing-type.component.html',
  styleUrls: ['./create-update-pricing-type.component.scss']
})
export class CreateUpdatePricingTypeComponent extends BaseComponent implements OnInit {

  edit = false;
  pricingType: PricingType = new PricingType();

  constructor(
    protected pricingTypeService: PricingTypeService,
    public config: DynamicDialogConfig,
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
      this.pricingType = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updatePricingType();
    } else {
      this.createPricingType();
    }
  }

  async updatePricingType() {
    if (!this.isValidData()) { return; }
    this.pricingType.isEnabled = true;
    this.pricingType.code = this.pricingType.code.trim();
    this.pricingType.name = this.pricingType.name.trim();
    let res = await this.pricingTypeService.update(this.pricingType)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }
  async createPricingType(){
    if (!this.isValidData()) { return; }
    this.pricingType.code = this.pricingType.code.trim();
    this.pricingType.name = this.pricingType.name.trim();
    let res = await this.pricingTypeService.create(this.pricingType)
    if (res.isSuccess) {
      this.msgService.success('Tạo công loại giá thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo công loại giá không thành công');
    }
  }

  isValidData(): boolean {
    
    if (!this.pricingType.code) {
      this.msgService.error('Vui lòng nhập mã loại giá');
      return false;
    }

    if (!this.pricingType.name) {
      this.msgService.error('Vui lòng nhập tên loại giá');
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
