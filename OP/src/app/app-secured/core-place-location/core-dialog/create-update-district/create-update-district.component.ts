import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { District } from 'src/app/shared/models/entity/district.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-district',
  templateUrl: './create-update-district.component.html',
  styleUrls: ['./create-update-district.component.scss']
})
export class CreateUpdateDistrictComponent extends BaseComponent implements OnInit {
  //
  districtItem: District = new District();
  //
  provinces: SelectModel[] = [];
  selectedProvince: SelectModel;
  edit = false;
  //
  constructor(
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected config: DynamicDialogConfig,
    //
    protected countryService: CountryService,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected router: Router,
    protected permissionService: PermissionService, 
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.initData();
  }

  async initData(): Promise<any> {
    await this.getProvinces();
    if (this.config.data) {
      this.edit = true;
      this.districtItem = this.config.data;
      const findCountry = this.provinces.find(f => f.value === this.districtItem.provinceId);
      if (findCountry) {
        this.selectedProvince = findCountry;
      }
    }
  }

  async getProvinces(): Promise<any> {
    this.provinces = await this.provinceService.getAllSelectModelAsync();
  }

  async createOrUpdate(): Promise<any> {
    if (this.isValidate()) { return; }
    if (this.config.data) {
      this.districtItem.code = this.districtItem.code.trim();
      this.districtItem.name = this.districtItem.name.trim(); 
      const res = await this.districtService.update(this.districtItem);
      if (res.isSuccess) {
        this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
        this.onClickCancel(true);
      } else {
        let a = Object.values(res.data)
        if(a){
          this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
        }else{
          this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
        }
      }
    } else {
      this.districtItem.kmNumber = 0;
      this.districtItem.lat = null;
      this.districtItem.lng = null;
      this.districtItem.code = this.districtItem.code.trim();
      this.districtItem.name = this.districtItem.name.trim();
      const res = await this.districtService.create(this.districtItem);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới quận huyện thành công');
        this.onClickCancel(true);
      } else {
        this.msgService.error(res.data.code || res.message );
      }
    }
  }

  onChangeCountry(): void {
    this.districtItem.provinceId = this.selectedProvince.value;
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
    if (!this.districtItem.code) {
      this.msgService.error('Mã quận huyện không được để trống');
      return true;
    }
    if (!this.districtItem.name) {
      this.msgService.error('Tên quận huyện không được để trống');
      return true;
    }
    if (!this.districtItem.provinceId) {
      this.msgService.error('Tỉnh thành không được để trống');
      return true;
    }
    return false;
  }
}
