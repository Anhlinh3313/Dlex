import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Reason } from 'src/app/shared/models/entity/reason.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ReasonService } from 'src/app/shared/services/api/reason.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-reason',
  templateUrl: './create-update-reason.component.html',
  styleUrls: ['./create-update-reason.component.scss']
})
export class CreateUpdateReasonComponent extends BaseComponent implements OnInit {

  edit = false;
  reason: Reason = new Reason();
  role: SelectModel[] = [];
  selectedRole: SelectModel;

  constructor(
    protected reasonService: ReasonService,
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
      this.reason = this.config.data;
      this.edit = true;
      const findBank = this.role.find(f => f.value === this.reason.roleId);
      if(findBank){
        this.selectedRole = findBank;
      }
    }
  }
  
  async loadRole(){
    this.role = await this.roleService.getRolesMultiSelectAsync();
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  onChangeRole(){
    this.reason.roleId = this.selectedRole.value;
  }

  onClickSave() {
    if (this.edit) {
      this.updateReason();
    } else {
      this.createReason();
    }
  }

  async updateReason(){
    if (!this.isValidData()) { return; }
    this.reason.isEnabled = true;
    this.reason.name = this.reason.name.trim();
    this.reason.code = this.reason.code.trim();
    let res = await this.reasonService.update(this.reason)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createReason(){
    if (!this.isValidData()) { return; }
    this.reason.code = this.reason.code.trim();
    this.reason.name = this.reason.name.trim();
    let res = await this.reasonService.create(this.reason)
    if (res.isSuccess) {
      this.msgService.success('Tạo dịch vụ thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo dịch vụ không thành công');
    }
  }

  isValidData(): boolean {
    
    if (!this.reason.code) {
      this.msgService.error('Vui lòng nhập mã lý do');
      return false;
    }

    if (!this.reason.name) {
      this.msgService.error('Vui lòng nhập tên lý do');
      return false;
    }

    return true;
  }
}
