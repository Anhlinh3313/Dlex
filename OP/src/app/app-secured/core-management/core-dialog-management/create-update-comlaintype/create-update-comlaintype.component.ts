import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { ComplainType } from 'src/app/shared/models/entity/complainType.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { ComplainTypeService } from 'src/app/shared/services/api/complainType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-comlaintype',
  templateUrl: './create-update-comlaintype.component.html',
  styleUrls: ['./create-update-comlaintype.component.scss']
})
export class CreateUpdateComlaintypeComponent extends BaseComponent implements OnInit {

  edit = false;
  role: SelectModel[] = [];
  selectedRole: SelectModel;
  complainType: ComplainType = new ComplainType();

  constructor(
    protected complainTypeService: ComplainTypeService,
    protected roleService: RoleService,
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
    await this.loadRole();
    if (this.config.data) {
      this.complainType = this.config.data;
      this.edit = true;
      const findrole = this.role.find(f => f.value === this.complainType.roleId);
      if (findrole) {
        this.selectedRole = findrole;
      }
    }
  }

  async loadRole() {
    this.role = await this.roleService.getRolesMultiSelectAsync();
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  onChangeRole(){
    this.complainType.roleId = this.selectedRole.value;
  }

  onClickSave() {
    if (this.edit) {
      this.updateComplainType();
    } else {
      this.createComplainType();
    }
  }

  async updateComplainType() {
    if (!this.isValidData()) { return; }
    this.complainType.isEnabled = true;
    this.complainType.name = this.complainType.name.trim();
    this.complainType.code = this.complainType.code.trim();
    let res = await this.complainTypeService.update(this.complainType)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createComplainType() {
    if (!this.isValidData()) { return; }
    this.complainType.code = this.complainType.code.trim();
    this.complainType.name = this.complainType.name.trim();
    let res = await this.complainTypeService.create(this.complainType)
    if (res.isSuccess) {
      this.msgService.success('Tạo loại khiếu nại thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo loại khiếu nại không thành công');
    }
   }

  isValidData(): boolean {
    
    if (!this.complainType.code) {
      this.msgService.error('Vui lòng nhập mã loại khiếu nại');
      return false;
    }

    if (!this.complainType.name) {
      this.msgService.error('Vui lòng nhập tên loại khiếu nại');
      return false;
    }

    // if (!this.selectedRole) {
    //   this.msgService.error('Vui lòng chọn quyền xử lý');
    //   return false;
    // }

    return true;
  }
}