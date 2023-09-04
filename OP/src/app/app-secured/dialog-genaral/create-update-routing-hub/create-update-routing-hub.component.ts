import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Hub } from 'src/app/shared/models/entity/Hub.model';
import { HubRouting } from 'src/app/shared/models/entity/hubRouting.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { GetDatasFromHubViewModel } from 'src/app/shared/models/viewModel/getDatasFromHub.viewModel';
import { HubRoutingWardViewModel } from 'src/app/shared/models/viewModel/hubRouting.viewModel';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { HubRoutingService } from 'src/app/shared/services/api/hubRouting.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-create-update-routing-hub',
  templateUrl: './create-update-routing-hub.component.html',
  styleUrls: ['./create-update-routing-hub.component.scss']
})
export class CreateUpdateRoutingHubComponent extends BaseComponent implements OnInit {

  first = 1;
  edit: boolean = false;
  hub: Hub = new Hub();
  poHubs: SelectModel[] = [];
  riders: SelectModel[] = [];
  provinces: SelectModel[] = [];
  districts: SelectModel[] = [];
  centerHubs: SelectModel[] = [];
  hubs: SelectModel[] = [];
  address: HubRoutingWardViewModel[] = [];
  SelectedWardIds: HubRoutingWardViewModel[] = [];
  loadSelectedWardIds: HubRoutingWardViewModel[] = [];
  loadWardIds: any;
  selectedCenterHub: SelectModel;
  selectedPoHub: SelectModel;
  selectedRider: SelectModel;
  selectedProvince: SelectModel;
  selectedDistrict: SelectModel;
  selectedHub: SelectModel;
  selectTypeRider: any;
  typeRider: SelectModel[] = [{ value: false, label: `Xe máy`, data: null }, { value: true, label: `Xe tải`, data: null }]
  centerHubId: number;
  poHubId: number;
  hubId: number;
  hubRouting: HubRouting = new HubRouting();
  checkSelectedHub = false;

  constructor(
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    protected msgService: MsgService,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected hubService: HubService,
    protected authService: AuthService,
    protected wardService: WardService,
    protected hubRoutingService: HubRoutingService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData(): Promise<any> {
    await this.getProvinces();
    await this.getCenterHub();
    if (this.config.data) {
      this.edit = true;
      this.hubRouting = this.config.data;
      const findCountryProvince = this.provinces.find(f => f.value === this.hubRouting.hub.provinceId);
      if (findCountryProvince) {
        this.selectedProvince = findCountryProvince;
        this.hub.provinceId = findCountryProvince.value;
        await this.getDistrictByProvinceId();
      }

      const findCountryCenterHub = this.centerHubs.find(f => f.value === this.hubRouting.hub.centerHubId);
      if (findCountryCenterHub) {
        this.selectedCenterHub = findCountryCenterHub;
        this.poHubs = await this.hubService.getSelectModelPoHubByCenterIdbAsync(this.selectedCenterHub.value);
      }

      const findCountryPoHub = this.poHubs.find(f => f.value === this.hubRouting.hub.poHubId);
      if (findCountryPoHub) {
        this.selectedPoHub = findCountryPoHub;
        this.hubs = await this.hubService.getSelectModelAsync(this.selectedPoHub.value);
      }

      const findCountryHub = this.hubs.find(f => f.value === this.hubRouting.hubId);
      if (findCountryHub) {
        this.selectedHub = findCountryHub;
        this.riders = await this.authService.getSelectModelEmpByCurrentHubAsync(this.selectedHub.value);
      }

      const findCountryRider = this.riders.find(f => f.value === this.hubRouting.userId);
      if (findCountryRider) {
        this.selectedRider = findCountryRider;
      }

      const findCountryTypeRider = this.typeRider.find(f => f.value === this.hubRouting.isTruckDelivery);
      if (findCountryTypeRider) {
        this.selectTypeRider = findCountryTypeRider.value;
      }
      this.hubId = this.hubRouting.hubId;
      await this.getWardIdsByHubId();
      await this.getDatasFromHubs();
    }
  }

  async getDatasFromHubs() {
    let res = await this.hubRoutingService.getDatasFromHub(this.hubId, this.hubRouting.id, this.hubRouting.isTruckDelivery);
    if (res.isSuccess) {
      this.address = res.data.wards;
      let obj = res.data as GetDatasFromHubViewModel;
      this.SelectedWardIds = obj.wards.filter(x => obj.selectedWardIds.indexOf(x.wardId) !== -1);
      let tableSelectedWardIds = this.loadWardIds.selectedWardIds.filter(x => obj.selectedWardIds.indexOf(x) === -1);
      this.address = this.address.filter(x => tableSelectedWardIds.indexOf(x.wardId) === -1);
    } else {
      this.address = []
    }
  }

  // onChangeProvince(): void {
  //   this.hub.provinceId = this.selectedProvince.value;
  //   this.getDistrictByProvinceId();
  // }

  async changeCenterHub(): Promise<void> {
    this.centerHubId = this.selectedCenterHub.value;
    this.poHubs = await this.hubService.getSelectModelPoHubByCenterIdbAsync(this.centerHubId);
    this.riders = [];
    this.hubs = [];
    this.address = [];
    this.selectedHub = null;
  }

  async onChangePoHub() {
    this.poHubId = this.selectedPoHub.value;
    if (this.poHubId) {
      this.riders = await this.authService.getSelectModelEmpByCurrentHubAsync(this.poHubId);
      this.hubs = await this.hubService.getSelectModelAsync(this.poHubId);
    } else {
      this.riders = [];
      this.hubs = [];
      this.address = [];
      this.selectedHub = null;
    }
  }

  async onChangeHub() {
    this.riders = null;
    this.hubId = this.selectedHub.value;
    this.hubRouting.hubId = this.selectedHub.value;
    let selectedHubId = this.selectedHub.value;
    if (this.hubId) {
      this.riders = await this.authService.getSelectModelEmpByCurrentHubAsync(selectedHubId);
    } else {
      this.riders = null;
    }
    await this.gethubRouting();
    await this.getWardIdsByHubId();
    this.checkSelectedHub = true;
    this.address = this.address.filter(x => this.loadWardIds.selectedWardIds.indexOf(x.id) === -1);
  }

  onChangeStationHub() {
    this.hubRouting.isTruckDelivery = this.selectTypeRider;
  }

  async gethubRouting(): Promise<any> {
    let res = await this.hubRoutingService.getWardByHubId(this.hubId);
    if (res.isSuccess) {
      this.address = res.data;
    } else {
      this.address = [];
    }
  }

  async getWardIdsByHubId(): Promise<any> {
    let res = await this.hubRoutingService.getWardIdsByHubId(this.hubId);
    if (res.isSuccess) {
      this.loadWardIds = res.data;
    } else {
      this.address = [];
    }
  }

  async getProvinces(): Promise<any> {
    this.provinces = await this.provinceService.getAllSelectModelAsync();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.districts = await this.districtService.getDistrictsSelectModelAsync(this.hub);
    if (this.edit) {
      this.selectedDistrict = this.districts.find(x => x.value == this.hubRouting.hub.districtId)
    } else {
      this.selectedDistrict.value = 0;
    }
  }

  async getCenterHub(): Promise<any> {
    this.centerHubs = await this.hubService.getCenterHubAsync();
  }

  async onClickSave() {
    if (this.edit) {
      await this.update();
    } else {
      await this.create();
    }
  }

  onChangeRider(): void {
    this.hubRouting.userId = this.selectedRider.value;
  }

  async update() {
    let wards = [];
    if (this.checkSelectedHub) {
      this.SelectedWardIds.forEach(x => wards.push(x.id));
    } else {
      this.SelectedWardIds.forEach(x => wards.push(x.wardId));
    }
    this.hubRouting.wardIds = wards;
    this.hubRouting.streetJoinIds = [];
    this.hubRouting.isEnabled = true;
    this.hubRouting.code = this.hubRouting.code.trim();
    this.hubRouting.name = this.hubRouting.name.trim();
    if (this.isValidate()) { return; }
    let res = await this.hubRoutingService.update(this.hubRouting)
    if (res.isSuccess) {
      this.onClickCancel(true);
      this.msgService.success("Cập nhật của bạn đã được thay đổi trên hệ thống");
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  async create() {
    if (this.isValidate()) { return; }
    let wards = [];
    if (this.checkSelectedHub) {
      this.SelectedWardIds.forEach(x => wards.push(x.id));
    } else {
      this.SelectedWardIds.forEach(x => wards.push(x.wardId));
    }
    this.hubRouting.wardIds = wards;
    this.hubRouting.streetJoinIds = [];
    this.hubRouting.isEnabled = true;
    this.hubRouting.code = this.hubRouting.code.trim();
    this.hubRouting.name = this.hubRouting.name.trim();
    let res = await this.hubRoutingService.create(this.hubRouting)
    if (res.isSuccess) {
      this.onClickCancel(true);
      this.msgService.success("Tạo mới phân tuyến giao nhận thành công");
    } else {
      this.msgService.error(res.data.code || res.message);
    }
  }

  onClickCancel(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  isValidate(): boolean {
    if (!this.hubRouting.code) {
      this.msgService.error('Vui lòng nhập mã phân tuyến');
      return true;
    }
    if (!this.hubRouting.name) {
      this.msgService.error('Vui lòng tên phân tuyến');
      return true;
    }
    // if (!this.selectedProvince) {
    //   this.msgService.error('Vui lòng chọn tỉnh thành');
    //   return true;
    // }
    // if (!this.selectedDistrict) {
    //   this.msgService.error('Vui lòng chọn quận huyện');
    //   return true;
    // }
    if (!this.selectedCenterHub) {
      this.msgService.error('Vui lòng chọn trung tâm');
      return true;
    }
    if (!this.selectedPoHub) {
      this.msgService.error('Vui lòng chọn chi nhánh');
      return true;
    }
    if (!this.selectedHub) {
      this.msgService.error('Vui lòng chọn kho trạm');
      return true;
    }
    if (!this.selectedRider) {
      this.msgService.error('Vui lòng chọn nhân viên phụ trách');
      return true;
    }
    if (!this.SelectedWardIds) {
      this.msgService.error('Vui lòng chọn danh sách phường xã');
      return true;
    }
    return false;
  }

}
