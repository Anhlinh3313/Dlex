import { MapsAPILoader } from '@agm/core';
import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { GMapHelper } from 'src/app/shared/infrastructure/gmap.helper';
import { District } from 'src/app/shared/models/entity/district.model';
import { Hub } from 'src/app/shared/models/entity/Hub.model';
import { InfoLocation } from 'src/app/shared/models/entity/infoLocation.model';
import { Province } from 'src/app/shared/models/entity/province.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { Ward } from 'src/app/shared/models/entity/ward.model';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-hub',
  templateUrl: './create-update-hub.component.html',
  styleUrls: ['./create-update-hub.component.scss']
})
export class CreateUpdateHubComponent extends BaseComponent implements OnInit {

  edit: boolean = false;
  selectedProvince: SelectModel;
  selectedDistrict: SelectModel;
  selectedWard: SelectModel;
  hub: Hub = new Hub();
  provinces: SelectModel[] = [];
  districts: SelectModel[] = [];
  wards: SelectModel[] = [];

  constructor(
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected wardService: WardService,
    protected hubService: HubService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,

  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData(): Promise<any> {
    await this.getProvinces();
    if (this.config.data) {
      this.hub = this.config.data;
      this.edit = true;
      const findCountryProvince = this.provinces.find(f => f.value === this.hub.provinceId);
      if (findCountryProvince) {
        this.selectedProvince = findCountryProvince;
        await this.getDistrictByProvinceId();
      }
      const findCountryDistrict = this.districts.find(f => f.value === this.hub.districtId);
      if (findCountryDistrict) {
        this.selectedDistrict = findCountryDistrict;
        await this.getWardByProvinceId();
      }
      const findCountryWard = this.wards.find(f => f.value === this.hub.wardId);
      if (findCountryWard) {
        this.selectedWard = findCountryWard;
      }
      this.hub.concurrencyStamp = this.config.data.concurrencyStamp;
    }
  }

  onChangeProvince(): void {
    this.hub.provinceId = this.selectedProvince.value;
    if (this.selectedProvince.value) {
      this.getDistrictByProvinceId();
    } else {
      this.districts = [];
    }
    this.wards = [];
  }

  onChangeDistrict() {
    this.hub.districtId = this.selectedDistrict.value;
    if (this.selectedDistrict.value) {
      this.getWardByProvinceId();
    } else {
      this.wards = [];
    }
  }

  onChangeWard() {
    this.hub.wardId = this.selectedWard.value;
  }

  async onClickSave() {
    if (this.isValidate()) { return; }
    if (this.edit) {
      this.update();
    } else {
      await this.create();
    }
  }

  async update() {
    this.hub.isEnabled = true;
    this.hub.code = this.hub.code.trim();
    this.hub.name = this.hub.name.trim();
    let res = await this.hubService.update(this.hub)
    if (res.isSuccess) {
      this.msgService.success("Cập nhật của bạn đã được thay đổi trên hệ thống");
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      if (a) {
        this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
      } else {
        this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
      }
    }
  }

  async create() {
    this.hub.code = this.hub.code.trim();
    this.hub.name = this.hub.name.trim();
    let res = await this.hubService.create(this.hub)
    if (res.isSuccess) {
      this.msgService.success("Tạo mới trung tâm thành công");
      this.onClickCancel(true);
    } else {
      this.msgService.error(res.data.code || res.message);
    }
  }

  async getProvinces(): Promise<any> {
    this.provinces = await this.provinceService.getAllSelectModelAsync();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.districts = await this.districtService.getDistrictsSelectModelAsync(this.hub);
  }

  async getWardByProvinceId(): Promise<any> {
    this.wards = await this.wardService.getWardsSelectModelAsync(this.hub);
  }

  isValidate(): boolean {
    if (!this.hub.name) {
      this.msgService.error('Vui lòng nhập tên trung tâm');
      return true;
    }
    if (!this.hub.code) {
      this.msgService.error('Vui lòng nhập mã trung tâm');
      return true;
    }
    if (!this.hub.provinceId) {
      this.msgService.error('Vui lòng chọn tỉnh thành');
      return true;
    }
    if (!this.hub.districtId) {
      this.msgService.error('Vui lòng chọn quận huyện');
      return true;
    }
    if (!this.hub.wardId) {
      this.msgService.error('Vui lòng chọn phường xã');
      return true;
    }
    return false;
  }

  onClickCancel(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  public setAddress(place: google.maps.places.PlaceResult) {
    if (!place.geometry || !place.geometry) {
      this.messageService.add({ severity: Constant.messageStatus.warn, detail: "Không tìm thấy địa chỉ" });
      return;
    }

    this.loadLocationPlace(place);
    this.hub.address = place.formatted_address;
    this.hub.lat = place.geometry.location.lat();
    this.hub.lng = place.geometry.location.lng();
  }

  async loadLocationPlace(place: google.maps.places.PlaceResult) {
    let provinceName = "";
    let districtName = "";
    let wardName = "";

    place.address_components.forEach(element => {
      if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
        provinceName = element.long_name;
      }
      else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1 || element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
        districtName = element.long_name;
      }
      else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1 || element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
        wardName = element.long_name;
      }
    });
    this.hub.provinceName = provinceName;
    this.hub.districtName = districtName;
    this.hub.ward = wardName;
    if (provinceName) {
      this.provinceService.getProvinceByName(provinceName).then(
        async x => {
          if (!super.isValidResponse(x)) return;
          let province = x.data as Province;
          if (!province) {
            this.selectedProvince = null;
            return;
          }
          const findProvince = this.provinces.find(f => f.value === province.id);
          this.selectedProvince = findProvince;
          this.hub.provinceId = this.selectedProvince.value;
          await this.getDistrictByProvinceId();
          if (districtName) {
            this.districtService.getDistrictByName(districtName, this.selectedProvince.value).then(
              async x => {
                if (!this.hub.districtName) {
                  this.districts = [];
                  this.wards = [];
                  this.messageService.add({
                    severity: Constant.messageStatus.warn,
                    detail: "Vui lòng chọn Quận/huyện gửi!"
                  });
                  this.messageService.add({
                    severity: Constant.messageStatus.warn,
                    detail: "Vui lòng chọn Phường/xã gửi!"
                  });
                } else {
                  if (!super.isValidResponse(x)) return;
                  let district = x.data as District;
                  if (!district) {
                    this.selectedDistrict = null;
                    return;
                  }
                  const findDistrict = this.districts.find(f => f.value === district.id);
                  this.selectedDistrict = findDistrict;
                  this.selectedWard = null;
                  this.hub.wardId = null;
                  await this.getDistrictByProvinceId();
                  this.hub.districtId = this.selectedDistrict.value;
                  await this.getWardByProvinceId();
                  if (wardName) {
                    this.wardService.getWardByName(wardName, this.selectedDistrict.value).then(
                      x => {
                        if (!this.hub.ward) {
                          this.wards = [];
                          this.messageService.add({
                            severity: Constant.messageStatus.warn,
                            detail: "Vui lòng chọn Phường/xã gửi!"
                          });
                        } else {
                          if (!super.isValidResponse(x)) return;
                          let ward = x.data as Ward;
                          if (!ward) {
                            this.selectedWard = null;
                            return;
                          }
                          const findWard = this.wards.find(f => f.value === ward.id);
                          this.selectedWard = findWard;
                          this.hub.wardId = this.selectedWard.value;
                        }
                      } //End wardService
                    );
                  }
                }
              } //End districtService
            );
          }
        } //End provinceService
      );
    }
  }

}
