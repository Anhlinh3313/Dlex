import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Country } from 'src/app/shared/models/entity/country.model';
import { Province } from 'src/app/shared/models/entity/province.model';
import { Role } from 'src/app/shared/models/entity/role.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-province',
  templateUrl: './create-update-province.component.html',
  styleUrls: ['./create-update-province.component.scss']
})
export class CreateUpdateProvinceComponent extends BaseComponent implements OnInit {
  //
  provinceItem: Province = new Province();
  //
  countries: SelectModel[] = [];
  selectedCountry: SelectModel = { value: null };
  edit = false;
  //
  constructor(
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected config: DynamicDialogConfig,
    //
    protected countryService: CountryService,
    protected provinceService: ProvinceService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.initData();
  }

  async initData(): Promise<any> {
    await this.getCountry();
    if (this.config.data) {
      this.edit = true;
      this.provinceItem = this.config.data;
      const findCountry = this.countries.find(f => f.value === this.provinceItem.countryId);
      if (findCountry) {
        this.selectedCountry = findCountry;
      }
    }
  }

  async getCountry(): Promise<any> {
    this.countries = await this.countryService.getAllSelectModelAsync();
  }

  async createOrUpdate(): Promise<any> {
    if (this.isValidate()) { return; }
    if (this.config.data) {
      this.provinceItem.code = this.provinceItem.code.trim();
      this.provinceItem.name = this.provinceItem.name.trim();
      const res = await this.provinceService.update(this.provinceItem);
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
      this.provinceItem.code = this.provinceItem.code.trim();
      this.provinceItem.name = this.provinceItem.name.trim();
      const res = await this.provinceService.create(this.provinceItem);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới tỉnh thành thành công');
        this.onClickCancel(true);
      } else {
        this.msgService.error(res.data.code);
      }
    }
  }

  onChangeCountry(): void {
    this.provinceItem.countryId = this.selectedCountry.value;
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
    if (!this.provinceItem.code) {
      this.msgService.error('Mã tỉnh thành không được để trống');
      return true;
    }
    if (!this.provinceItem.name) {
      this.msgService.error('Tên tỉnh thành không được để trống');
      return true;
    }
    if (!this.provinceItem.countryId) {
      this.msgService.error('Quốc gia không được để trống');
      return true;
    }
    return false;
  }
}
