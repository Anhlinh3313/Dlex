import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { FromPrint } from 'src/app/shared/models/entity/fromPrint.model';
import { FormPrintService } from 'src/app/shared/services/api/formPrintType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-print-form',
  templateUrl: './create-print-form.component.html',
  styleUrls: ['./create-print-form.component.scss']
})
export class CreatePrintFormComponent extends BaseComponent implements OnInit {

  name: string;
  code; string;
  numOrder: number;
  data: FromPrint = new FromPrint();
  formPrintType: number

  constructor(
    protected formPrintService: FormPrintService,
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

  intData() {
    if (this.config.data) {
      this.formPrintType = this.config.data;
    }
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  async onClickSave() {
    if (!this.isValidData()) { return; }
    this.data.code = this.code.trim();
    this.data.name = this.name.trim();
    this.data.isPublic = false;
    this.data.formPrintBody = "";
    this.data.formPrintTypeId = this.formPrintType;
    this.data.numOrder = this.numOrder;
    this.formPrintService.create(this.data).then(res => {
      if (res.isSuccess) {
        this.msgService.success('Tạo mẫu in thành công');
        this.onClickCancel(true);
      } else {
        let a = Object.values(res.data)
        this.msgService.error(a.toString() || 'Tạo mẫu in thành công');
      }
    }, resError => {
      if (resError) {
        this.msgService.error(resError.error.code[0]);
      } else {
        this.msgService.error('Đã có lỗi xảy ra! Vui lòng thử lại!');
      }
    })
  }

  isValidData(): boolean {

    if (!this.code) {
      this.msgService.error('Vui lòng nhập mã mẫu in');
      return false;
    }

    if (!this.name) {
      this.msgService.error('Vui lòng nhập tên mẫu in');
      return false;
    }
    return true;
  }
}
