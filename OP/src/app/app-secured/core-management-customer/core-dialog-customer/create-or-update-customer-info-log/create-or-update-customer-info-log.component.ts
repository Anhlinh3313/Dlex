import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { GooglePlaceDirective } from 'ngx-google-places-autocomplete';
import { Address } from 'ngx-google-places-autocomplete/objects/address';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { GMapHelper } from 'src/app/shared/infrastructure/gmap.helper';
import { CustomerInfoLog } from 'src/app/shared/models/entity/customerInfoLog.model';
import { District } from 'src/app/shared/models/entity/district.model';
import { Province } from 'src/app/shared/models/entity/province.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { Ward } from 'src/app/shared/models/entity/ward.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CustomerInfoLogService } from 'src/app/shared/services/api/customerInfoLog.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-or-update-customer-info-log',
  templateUrl: './create-or-update-customer-info-log.component.html',
  styleUrls: ['./create-or-update-customer-info-log.component.scss']
})
export class CreateOrUpdateCustomerInfoLogComponent extends BaseComponent implements OnInit {

  @ViewChild('placesRef') placesRef: GooglePlaceDirective;
  options = {
    types: [],
    componentRestrictions: { country: 'VN' }
  };
  edit = false;
  provinces: SelectModel[] = [];
  districts: SelectModel[] = [];
  wards: SelectModel[] = [];
  customerInfoLog: CustomerInfoLog = new CustomerInfoLog();
  selectedProvince: SelectModel;
  selectWard: SelectModel;
  filterViewModel: FilterViewModel = {
    pageNumber: 1,
    pageSize: 100,
    provinceId: null
  };
  selectedDistrict: SelectModel;
  geocoder: any;
  adr_address: string;
  constructor(
    protected customerInfoLogService: CustomerInfoLogService,
    protected wardService: WardService,
    protected districtService: DistrictService,
    protected provinceService: ProvinceService,
    public hubService: HubService,
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    public zone: NgZone,

  ) {
    super(msgService, router, permissionService);
    if (google.maps.places) {
      this.geocoder = new google.maps.Geocoder;
    }
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    await this.getProvince();
    if (this.config.data) {
      this.customerInfoLog = this.config.data;
      this.edit = true;
      const findProvince = this.provinces.find(f => f.value === this.customerInfoLog.provinceId);
      if (findProvince) {
        this.selectedProvince = findProvince;
      }
      await this.onchangeProvince();
      const findDistrict = this.districts.find(f => f.value === this.customerInfoLog.districtId);
      if (findDistrict) {
        this.selectedDistrict = findDistrict;
      }
      await this.getWardByDistrictId();
      const findWard = this.wards.find(f => f.value === this.customerInfoLog.wardId);
      if (findWard) {
        this.selectWard = findWard;
      }
    }
  }

  async getProvince(): Promise<any> {
    this.provinces = await this.provinceService.getProvinceAsync();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.filterViewModel.provinceId = this.selectedProvince.value;
    this.districts = await this.districtService.getDistrictsSelectModelAsync(this.filterViewModel);
  }

  async getWardByDistrictId(): Promise<any> {
    this.selectWard = null;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    this.filterViewModel.districtid = this.selectedDistrict.value;
    this.wards = await this.wardService.getWardsSelectModelAsync(this.filterViewModel)
  }

  async onchangeProvince(): Promise<void> {
    this.customerInfoLog.provinceId = this.selectedProvince.value;
    this.selectedDistrict = null;
    this.selectWard = null;
    if (this.selectedProvince.value) {
      await this.getDistrictByProvinceId();
    } else {
      this.districts = [];
    }
    this.wards = [];
  }

  async onChangeDistrict(): Promise<void> {
    this.customerInfoLog.districtId = this.selectedDistrict.value;
    this.selectWard = null;
    if (this.selectedDistrict.value) {
      await this.getWardByDistrictId();
    } else {
      this.wards = []
    }
  }

  onchangeWard() {
    this.customerInfoLog.wardId = this.selectWard.value;
  }

  onClickSave() {
    if (this.edit) {
      this.updateCustomerInfoLog();
    } else {
      this.createCustomerInfoLog();
    }
  }

  async updateCustomerInfoLog() {
    if (!this.isValidData()) { return; }
    this.customerInfoLog.isEnabled = true;
    this.customerInfoLog.code = this.customerInfoLog.name.trim();
    this.customerInfoLog.name = this.customerInfoLog.name.trim();
    this.customerInfoLog.address = this.customerInfoLog.address.trim();
    if (this.customerInfoLog.addressNote) {
      this.customerInfoLog.addressNote = this.customerInfoLog.addressNote.trim();
    }
    let res = await this.customerInfoLogService.update(this.customerInfoLog)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data)
      if (a) {
        this.msgService.error('Thông tin khách nhận đã tồn tại');
      } else {
        this.msgService.error(a.toString() || 'Cập nhật của bạn không thành công!');
      }
    }
  }

  async createCustomerInfoLog() {
    if (!this.isValidData()) { return; }
    this.customerInfoLog.code = this.customerInfoLog.name.trim();
    this.customerInfoLog.name = this.customerInfoLog.name.trim();
    this.customerInfoLog.address = this.customerInfoLog.address.trim();
    if (this.customerInfoLog.addressNote) {
      this.customerInfoLog.addressNote = this.customerInfoLog.addressNote.trim();
    }
    let res = await this.customerInfoLogService.create(this.customerInfoLog)
    if (res.isSuccess) {
      this.msgService.success('Tạo thông tin khách nhận thành công');
      this.onClickCancel(true);
    } else {
      let a = Object.values(res.data);
      if (a) {
        this.msgService.error('Thông tin khách nhận đã tồn tại');
      } else {
        this.msgService.error(a.toString() || 'Tạo thông tin khách nhận không thành công');
      }
    }
  }

  isValidData(): boolean {

    if (!this.customerInfoLog.name) {
      this.msgService.error('Vui lòng nhập tên khách nhận');
      return false;
    }

    if (!this.customerInfoLog.phoneNumber) {
      this.msgService.error('Vui lòng nhập số điện thoại khách nhận');
      return false;
    }

    if (!this.customerInfoLog.address) {
      this.msgService.error('Vui lòng nhập địa chỉ khách nhận');
      return false;
    }

    if (!this.selectedProvince) {
      this.msgService.error('Vui lòng chọn tỉnh thành khách nhận');
      return false;
    }

    if (!this.selectedDistrict) {
      this.msgService.error('Vui lòng chọn quận huyện khách nhận');
      return false;
    }

    if (!this.selectWard) {
      this.msgService.error('Vui lòng nhập phường xã khách nhận');
      return false;
    }
    return true;
  }

  onClickCancel(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  async handleAddressChange(address: Address, inputAddress): Promise<any> {
    this.selectedAddressItem(address, inputAddress);
  }

  selectedAddressItem(prediction, inputAddress): void {
    if (inputAddress.id === 'address') {
      this.customerInfoLog.address = prediction.formatted_address;
    }
    this.geocoder.geocode({ placeId: prediction.place_id }, (resultsPlaceId, statusPlaceId) => {
      if (statusPlaceId === 'OK' && resultsPlaceId[0]) {
        const latlng = resultsPlaceId[0].geometry.location;
        this.geocoder.geocode({ placeId: resultsPlaceId[0].place_id }, (resultsLatLng, statusLatLng) => {
          if (statusLatLng === 'OK' && resultsLatLng[0]) {
            this.loadInitMap(resultsLatLng, inputAddress);
          }
        });
      }
    });
    this.adr_address = prediction.adr_address;
  }

  async loadInitMap(resultsLatLng, inputAddress): Promise<any> {
    const results = resultsLatLng[0];
    const lat = results.geometry.location.lat();
    const lng = results.geometry.location.lng();

    let provinceName; let districtName; let wardName;
    results.address_components.map(element => {
      //
      if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
        provinceName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
        districtName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1
        && element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) === -1) {
        districtName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1) {
        wardName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
        wardName = element.long_name;
      }
    });

    if (!wardName) {
      this.adr_address.split(",").map(i => {
        if (i.includes("extended-address")) {
          i = i.slice(32);
          i = i.replace("</span>", "");
          wardName = i;
        }
      })
    }

    if (inputAddress.id === 'address') {
      this.zone.run(async () => {
        const res = await this.hubService.getInfoLocation(provinceName, districtName, wardName);
        if (res) {
          const findProvince = this.provinces.find(f => f.value === res.provinceId);
          if (findProvince) {
            this.selectedProvince = findProvince;
            await this.getDistrictByProvinceId();
            const findDistrict = this.districts.find(f => f.value === res.districtId);
            if (findDistrict) {
              this.selectedDistrict = findDistrict;
              await this.getWardByDistrictId();
              const findWard = this.wards.find(f => f.value === res.wardId);
              if (findWard && wardName) {
                this.selectWard = findWard;
              }
            }
            // if (res.hubId) {
            //   this.customer.hubId = res.hubId;
            // }
          }
          this.customerInfoLog.provinceId = res.provinceId;
          this.customerInfoLog.districtId = res.districtId;
          this.customerInfoLog.wardId = res.wardId;
        }
      });
    }
  }

}
