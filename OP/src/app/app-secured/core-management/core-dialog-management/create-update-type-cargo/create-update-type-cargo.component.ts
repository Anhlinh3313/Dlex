import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Structure } from 'src/app/shared/models/entity/structure.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { StructureService } from 'src/app/shared/services/api/Structure.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-type-cargo',
  templateUrl: './create-update-type-cargo.component.html',
  styleUrls: ['./create-update-type-cargo.component.scss']
})
export class CreateUpdateTypeCargoComponent extends BaseComponent implements OnInit {

  structure: Structure = new Structure();
  edit = false;

  constructor(
    protected structureService: StructureService,
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
      this.structure = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updateStructure();
    } else {
      this.createStructure();
    }
  }

  async updateStructure() {
    if (!this.isValidData()) { return; }
    this.structure.isEnabled = true;
    this.structure.code = this.structure.code.trim();
    this.structure.name = this.structure.name.trim();
    let res = await this.structureService.update(this.structure)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }
  async createStructure() {
    if (!this.isValidData()) { return; }
    this.structure.code = this.structure.code.trim();
    this.structure.name = this.structure.name.trim();
    let res = await this.structureService.create(this.structure)
    if (res.isSuccess) {
      this.msgService.success('Tạo loại hàng hoá thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo loại hàng hoá không thành công');
    }
  }

  isValidData(): boolean {

    if (!this.structure.code) {
      this.msgService.error('Vui lòng nhập mã loại hàng hoá');
      return false;
    }

    if (!this.structure.name) {
      this.msgService.error('Vui lòng nhập tên loại hàng hoá');
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
