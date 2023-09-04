import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { TransportType } from 'src/app/shared/models/entity/transportType.model';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { TransportTypeService } from 'src/app/shared/services/api/transportType.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-type-shipping',
  templateUrl: './create-update-type-shipping.component.html',
  styleUrls: ['./create-update-type-shipping.component.scss']
})
export class CreateUpdateTypeShippingComponent extends BaseComponent implements OnInit {

  transportType: TransportType = new TransportType();
  edit = false;

  constructor(
    protected transportTypeService: TransportTypeService,
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
      this.transportType = this.config.data;
      this.edit = true;
    }
  }

  onClickSave() {
    if (this.edit) {
      this.updateTransportType();
    } else {
      this.createTransportType();
    }
  }

  async updateTransportType(){
    if (!this.isValidData()) { return; }
    this.transportType.isEnabled = true;
    this.transportType.code = this.transportType.code.trim();
    this.transportType.name = this.transportType.name.trim();
    let res = await this.transportTypeService.update(this.transportType)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
    }
  }

  async createTransportType(){
    if (!this.isValidData()) { return; }
    this.transportType.code = this.transportType.code.trim();
    this.transportType.name = this.transportType.name.trim();
    let res = await this.transportTypeService.create(this.transportType)
    if (res.isSuccess) {
      this.msgService.success('Tạo loại vận chuyển thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      this.msgService.error(a.toString() || 'Tạo loại vận chuyển không thành công');
    }
  }

  isValidData(): boolean {

    if (!this.transportType.code) {
      this.msgService.error('Vui lòng nhập mã loại vận chuyển');
      return false;
    }

    if (!this.transportType.name) {
      this.msgService.error('Vui lòng nhập tên loại vận chuyển');
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
