import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Service } from 'src/app/shared/models/entity/service.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ServiceService } from 'src/app/shared/services/api/service.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-service',
  templateUrl: './create-update-service.component.html',
  styleUrls: ['./create-update-service.component.scss']
})
export class CreateUpdateServiceComponent extends BaseComponent implements OnInit {

  servive: Service = new Service();
  edit = false;

  constructor(
    protected serviceService: ServiceService,
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) { 
    super(msgService,router, permissionService);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    if (this.config.data) {
      this.servive = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updateService();
    } else {
      this.createService();
    }
  }

  async updateService(){
    if (!this.isValidData()) { return; }
    this.servive.isEnabled = true; 
    this.servive.code = this.servive.code.trim();
    this.servive.name = this.servive.name.trim();
    let res = await this.serviceService.update(this.servive)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createService(){
    if (!this.isValidData()) { return; }
    this.servive.code = this.servive.code.trim();
    this.servive.name = this.servive.name.trim();
    let res = await this.serviceService.create(this.servive)
    if (res.isSuccess) {
      this.msgService.success('Tạo dịch vụ thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo dịch vụ không thành công');
    }
  }

  isValidData(): boolean {
    
    if (!this.servive.code) {
      this.msgService.error('Vui lòng nhập mã dịch vụ');
      return false;
    }

    if (!this.servive.name) {
      this.msgService.error('Vui lòng nhập tên dịch vụ');
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
