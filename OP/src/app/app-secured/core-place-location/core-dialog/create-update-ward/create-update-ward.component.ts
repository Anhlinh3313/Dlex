import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { District } from 'src/app/shared/models/entity/district.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { Ward } from 'src/app/shared/models/entity/ward.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-ward',
  templateUrl: './create-update-ward.component.html',
  styleUrls: ['./create-update-ward.component.scss']
})
export class CreateUpdateWardComponent extends BaseComponent implements OnInit {
  //
  wardItem: Ward = new Ward();
  filterDistrict: FilterViewModel = {
    pageNumber: 1,
    pageSize: 100,
    provinceId: null
  };
  //
  provinces: SelectModel[] = [];
  selectedProvince: SelectModel;
  districts: SelectModel[] = [];
  selectedDistrict: SelectModel;
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
    protected wardService: WardService,
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
      this.wardItem = this.config.data;
      const findProvince = this.provinces.find(f => f.value === this.wardItem.provinceId);
      if (findProvince) {
        this.selectedProvince = findProvince;
      }
      await this.onChangeProvince();
      const findDistrict = this.districts.find(f => f.value === this.wardItem.districtId);
      if (findDistrict) {
        this.selectedDistrict = findDistrict;
      }
    }
  }

  async getProvinces(): Promise<any> {
    this.provinces = await this.provinceService.getAllSelectModelAsync();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.filterDistrict.provinceId = this.selectedProvince.value;
    this.districts = await this.districtService.getDistrictsSelectModelAsync(this.filterDistrict);
  }

  async createOrUpdate(): Promise<any> {
    if (this.isValidate()) { return; }
    if (this.config.data) {
      this.wardItem.code = this.wardItem.code.trim();
      this.wardItem.name = this.wardItem.name.trim();
      const res = await this.wardService.update(this.wardItem);
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
      this.wardItem.kmNumber = 0;
      this.wardItem.lat = null;
      this.wardItem.lng = null;
      this.wardItem.code = this.wardItem.code.trim();
      this.wardItem.name = this.wardItem.name.trim();
      const res = await this.wardService.create(this.wardItem);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới phường xã thành công');
        this.onClickCancel(true);
      } else {
        this.msgService.error(res.data.code || res.message);
      }
    }
  }

  async onChangeProvince(): Promise<void> {
    this.selectedDistrict = null;
    this.wardItem.provinceId = this.selectedProvince.value;
    await this.getDistrictByProvinceId();
  }

  async onChangeDistrict(): Promise<any> {
    this.wardItem.districtId = this.selectedDistrict.value;
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
    if (!this.wardItem.code) {
      this.msgService.error('Mã phường xã không được để trống');
      return true;
    }
    if (!this.wardItem.name) {
      this.msgService.error('Tên phường xã không được để trống');
      return true;
    }
    if (!this.wardItem.provinceId) {
      this.msgService.error('Tỉnh thành không được để trống');
      return true;
    }
    if (!this.wardItem.districtId) {
      this.msgService.error('Quận huyện không được để trống');
      return true;
    }
    return false;
  }
}
