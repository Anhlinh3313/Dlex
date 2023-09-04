import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Country } from 'src/app/shared/models/entity/country.model';
import { Role } from 'src/app/shared/models/entity/role.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-country',
  templateUrl: './create-update-country.component.html',
  styleUrls: ['./create-update-country.component.scss']
})
export class CreateUpdateCountryComponent extends BaseComponent implements OnInit {
  //
  countryItem: Country = new Country();
  //
  parrentRoles: SelectModel[] = [];
  selectedParrentRole: any;
  edit = false;
  //
  constructor(
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected config: DynamicDialogConfig,
    //
    protected countryService: CountryService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);

  }

  ngOnInit(): void {
    if (this.config.data) {
      this.edit = true;
      this.countryItem = this.config.data;
    }
  }
  async createOrUpdate(): Promise<any> {
    if (this.isValidate()) { return; }
    if (this.config.data) {
      this.countryItem.code = this.countryItem.code.trim();
      this.countryItem.name = this.countryItem.name.trim();
      const res = await this.countryService.update(this.countryItem);
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
      this.countryItem.code = this.countryItem.code.trim();
      this.countryItem.name = this.countryItem.name.trim();
      const res = await this.countryService.create(this.countryItem);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới quốc gia thành công');
        this.onClickCancel(true);
      } else {
        this.msgService.error(res.data.code);
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
    if (!this.countryItem.code) {
      this.msgService.error('Mã quốc gia không được để trống');
      return true;
    }
    if (!this.countryItem.name) {
      this.msgService.error('Tên quốc gia không được để trống');
      return true;
    }
    return false;
  }
}
