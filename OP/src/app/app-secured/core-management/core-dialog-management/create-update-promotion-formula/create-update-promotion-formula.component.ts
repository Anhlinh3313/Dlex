import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PromotionFormula } from 'src/app/shared/models/entity/promotionFormula.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PromotionFormulaService } from 'src/app/shared/services/api/promotionFormula.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-promotion-formula',
  templateUrl: './create-update-promotion-formula.component.html',
  styleUrls: ['./create-update-promotion-formula.component.scss']
})
export class CreateUpdatePromotionFormulaComponent extends BaseComponent implements OnInit {

  promotionFormula: PromotionFormula = new PromotionFormula();
  edit = false;

  constructor(
    protected promotionFormulaService: PromotionFormulaService,
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
      this.promotionFormula = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updatePromotionFormula();
    } else {
      this.createPromotionFormula();
    }
  }

  async updatePromotionFormula() {
    if (!this.isValidData()) { return; }
    this.promotionFormula.isEnabled = true;
    this.promotionFormula.code = this.promotionFormula.code.trim();
    this.promotionFormula.name = this.promotionFormula.name.trim();
    let res = await this.promotionFormulaService.update(this.promotionFormula)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }
  async createPromotionFormula() {
    if (!this.isValidData()) { return; }
    this.promotionFormula.code = this.promotionFormula.code.trim();
    this.promotionFormula.name = this.promotionFormula.name.trim();
    let res = await this.promotionFormulaService.create(this.promotionFormula)
    if (res.isSuccess) {
      this.msgService.success('Tạo công thức giảm giá thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo công thức giảm giá không thành công');
    }
  }

  isValidData(): boolean {

    if (!this.promotionFormula.code) {
      this.msgService.error('Vui lòng nhập mã công thức giảm giá');
      return false;
    }

    if (!this.promotionFormula.name) {
      this.msgService.error('Vui lòng nhập tên công thức giảm giá');
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
