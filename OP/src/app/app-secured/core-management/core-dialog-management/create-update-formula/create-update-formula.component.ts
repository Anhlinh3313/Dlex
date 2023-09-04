import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Formula } from 'src/app/shared/models/entity/formula.model';
import { FormulaService } from 'src/app/shared/services/api/formula.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-formula',
  templateUrl: './create-update-formula.component.html',
  styleUrls: ['./create-update-formula.component.scss']
})
export class CreateUpdateFormulaComponent extends BaseComponent implements OnInit {

  edit = false;
  formula: Formula = new Formula();

  constructor(
    protected formulaService: FormulaService,
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
      this.formula = this.config.data;
      this.edit = true;
    }
  }

  
  onClickSave() {
    if (this.edit) {
      this.updateFormula();
    } else {
      this.createFormula();
    }
  }

  async updateFormula() {
    if (!this.isValidData()) { return; }
    this.formula.isEnabled = true;
    this.formula.code = this.formula.code.trim();
    this.formula.name = this.formula.name.trim();
    let res = await this.formulaService.update(this.formula)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }
  async createFormula(){
    if (!this.isValidData()) { return; }
    this.formula.code = this.formula.code.trim();
    this.formula.name = this.formula.name.trim();
    let res = await this.formulaService.create(this.formula)
    if (res.isSuccess) {
      this.msgService.success('Tạo công thức tính giá thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo công thức tính giá không thành công');
    }
  }

  isValidData(): boolean {
    
    if (!this.formula.code) {
      this.msgService.error('Vui lòng nhập mã công thức tính giá');
      return false;
    }

    if (!this.formula.name) {
      this.msgService.error('Vui lòng nhập tên công thức tính giá');
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
