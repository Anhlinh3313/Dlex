import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Role } from 'src/app/shared/models/entity/role.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-role',
  templateUrl: './create-update-role.component.html',
  styleUrls: ['./create-update-role.component.scss']
})
export class CreateUpdateRoleComponent extends BaseComponent implements OnInit {
  //
  roleItem: Role = new Role();
  //
  parrentRoles: SelectModel[] = [];
  selectedParrentRole: any;
  edit = false;
  //
  constructor(
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected config: DynamicDialogConfig,
    protected router: Router,
    protected permissionService: PermissionService,
    protected roleService: RoleService,
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    if (this.config.data) {
      this.edit = true;
      this.roleItem = this.config.data;
    }
  }
  async createOrUpdate(): Promise<any> {
    if (this.isValidate()) { return; }
    if (this.config.data) {
      const res = await this.roleService.update(this.roleItem);
      if (res.isSuccess) {
        this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
        this.onClickCancel(true);
      } else {
        let a = Object.values(res.data)
        if (a) {
          this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
        } else {
          this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
        }
      }
    } else {
      const res = await this.roleService.create(this.roleItem);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới chức vụ thành công');
        this.onClickCancel(true);
      } else {
        this.msgService.error(res.data.code || res.data.name);
      }
    }
  }

  onClick(): void {
    this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
  }

  onClickCancel(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  isValidate(): boolean {
    if (!this.roleItem.code) {
      this.msgService.error('Mã chức vụ không được để trống');
      return true;
    }
    if (!this.roleItem.name) {
      this.msgService.error('Tên chức vụ không được để trống');
      return true;
    }
    if (this.roleItem.code.trim() == "PICKUP") {
      //không được tạo trùng với mã chức vụ nhân viên giao hàng 6,9,31
      this.msgService.error('Mã chức không được thao tác');
      return true;
    }
    if (this.roleItem.code.trim() == "accounting") {
      this.msgService.error('Mã chức không được thao tác');
      return true;
    }
    if (this.roleItem.code.trim() == "DELIVERY") {
      this.msgService.error('Mã chức không được thao tác');
      return true;
    }
    return false;
  }
}
