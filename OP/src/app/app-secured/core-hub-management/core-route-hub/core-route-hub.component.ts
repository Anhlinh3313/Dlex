import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { District } from 'src/app/shared/models/entity/district.model';
import { Province } from 'src/app/shared/models/entity/province.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { Ward } from 'src/app/shared/models/entity/ward.model';
import { DataHubRouteViewModel } from 'src/app/shared/models/viewModel/dataHubRoute.viewModel';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { HubRouteService } from 'src/app/shared/services/api/hubRoute.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-route-hub',
  templateUrl: './core-route-hub.component.html',
  styleUrls: ['./core-route-hub.component.scss']
})
export class CoreRouteHubComponent extends BaseComponent implements OnInit {

  firstPage1 = 0;
  firstPage2 = 0;
  firstPage3 = 0;
  centerHubs: SelectModel[] = [];
  poHubs: SelectModel[] = [];
  hubs: SelectModel[] = [];
  selectedCenterHub: SelectModel;
  selectedPoHub: SelectModel;
  selectedHub: SelectModel;
  centerHubId: number;
  selectedProvinces: Province[] = [];
  provinces: Province[] = [];
  districts: District[] = [];
  selectedDistricts: District[] = [];
  wards: Ward[] = [];
  selectedWards: Ward[] = [];

  colsProvince = [
    { field: 'name', header: 'Tỉnh thành' },
  ];
  colsDistrict = [
    { field: 'name', header: 'Quận huyện' },
    { field: 'province.name', header: 'Tỉnh thành' },
  ];
  colsWard = [
    { field: 'name', header: 'Phường xã' },
    { field: 'district.name', header: 'Quận huyện' },
    { field: 'district.province.name', header: 'Tỉnh thành' }
  ];

  constructor(
    protected hubService: HubService,
    protected hubRouteService: HubRouteService,
    protected wardService: WardService,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected msgService: MsgService,
    public router: Router,
    public permissionService: PermissionService,
    protected breadcrumbService: BreadcrumbService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Quản phân khu vực phục vụ' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.getCenterHub();
  }

  async getCenterHub(): Promise<any> {
    this.centerHubs = await this.hubService.getCenterHubAsync();
    this.provinces = await this.provinceService.getAllAsync()
  }

  async changeCenterHub() {
    this.selectedProvinces = null;
    this.districts = [];
    this.wards = [];
    this.centerHubId = this.selectedCenterHub.value;
    this.poHubs = await this.hubService.getSelectModelPoHubByCenterIdbAsync(this.centerHubId);
    this.hubs = [];
  }

  async changePoHub() {
    this.selectedProvinces = null;
    this.districts = [];
    this.wards = [];
    this.centerHubId = this.selectedPoHub.value;
    this.hubs = await this.hubService.getSelectModelAsync(this.centerHubId);
    this.selectedHub = null;
  }

  changeHub() {
    this.hubRouteService.getDatasFromHub(this.selectedHub.value).then(
      async x => {
        let dataHubRoute = x.data as DataHubRouteViewModel;
        if (dataHubRoute !== null) {
          this.selectedProvinces = this.provinces.filter(r => dataHubRoute.selectedProvinceCiyIds.indexOf(r.id) !== -1);
          dataHubRoute.districts.forEach(x => x.province = this.selectedProvinces.filter(p => p.id === x.provinceId)[0]);
          this.districts = dataHubRoute.districts;
          this.selectedDistricts = this.districts.filter(r => dataHubRoute.selectedDistrictIds.indexOf(r.id) !== -1);
          let cols = [
            Constant.classes.includes.ward.district
          ];
          let districtIds = this.selectedDistricts.map(x => x.id);
          this.wardService.getWardByDistrictIds(districtIds, false, cols).then(res => {
            this.wards = res.data as Ward[];
            this.wards.map(ward => ward.district.province = this.provinces.filter(pro => pro.id == ward.district.provinceId)[0]);
            let wardIds = this.wards.map(x => x.id);
            this.hubRouteService.GetHubRouteByWardIds(wardIds, this.selectedHub.value).then(res => {
              let data = res.data as any[];
              this.wards.map(ward => {
                let find = data.find(hub => hub.wardId == ward.id);
                if (find) {
                  ward["isDisabled"] = true;
                  ward["hubName"] = find["name"];
                }
              })
              dataHubRoute.wards.forEach(x => x.district = this.selectedDistricts.filter(p => p.id === x.districtId)[0]);
              setTimeout(() => {
                this.selectedWards = dataHubRoute.wards;
              }, 0);
            });
          })
        } else {
          this.provinces = await this.provinceService.getAllAsync()
          this.districts = [];
          this.wards = [];
          this.selectedProvinces = [];
        }
      })
  }

  async clickSelectProvinces() {
    this.firstPage2 = 0;
    this.firstPage3 = 0;
    if (this.isValidateProvinces()) { return; }
    let ids: number[] = [];
    this.selectedProvinces.forEach(x => ids.push(x.id));
    const districts = await this.districtService.getDistrictByProvinceIdsAsync(ids);
    if (districts) {
      this.districts = districts as District[];
      this.districts.forEach(
        x => {
          x.province = this.selectedProvinces.filter(dest => dest.id === x.provinceId)[0];
        }
      );
    }
    this.selectedDistricts = this.selectedDistricts.filter(dist => this.selectedProvinces.find(pro => pro.id == dist.provinceId));
    this.selectedWards = this.selectedWards.filter(ward => this.selectedDistricts.find(dist => dist.id == ward.districtId && dist.provinceId == ward.district.provinceId));
    this.wards = this.wards.filter(ward => this.selectedProvinces.find(pro => pro.id == ward.district.provinceId) && this.selectedDistricts.find(dist => dist.id == ward.districtId));
  }

  async clickSelectDistricts() {
    this.firstPage1 = 0;
    this.firstPage3 = 0;
    if (this.isValidateDistricts()) { return; }
    let ids: number[] = [];
    this.selectedDistricts.forEach(x => ids.push(x.id));
    const wards = await this.wardService.getWardByDistrictIdsAsync(ids, true);
    if (wards) {
      this.wards = wards as Ward[];
      this.wards.forEach(
        x => {
          x.district = this.selectedDistricts.filter(dest => dest.id === x.districtId)[0];
        }
      );
      let wardIds = this.wards.map(x => x.id);
      const data = await this.hubRouteService.getHubRouteByWardIdsAsync(wardIds, this.selectedHub.value);
      if (data) {
        this.wards.map(ward => {
          let find = data.find(hub => hub.wardId == ward.id);
          if (find) {
            ward["isDisabled"] = true;
            ward["hubName"] = find["name"];
          }
        })
        this.selectedWards = this.selectedWards.filter(ward => this.selectedDistricts.find(dist => dist.id == ward.districtId && dist.provinceId == ward.district.provinceId));
        this.wards = this.wards.filter(ward => this.selectedProvinces.find(pro => pro.id == ward.district.provinceId) && this.selectedDistricts.find(dist => dist.id == ward.districtId));
      }
    }
  }

  clickSaveChange() {
    this.firstPage1 = 0;
    this.firstPage2 = 0;
    this.firstPage3 = 0;
    if (!this.selectedHub) {
      this.msgService.error('Vui lòng chọn trạm');
      return;
    }
    let wardIds = [];
    this.selectedWards.forEach(x => wardIds.push(x.id));
    this.hubRouteService.saveChangeHubRoute(this.selectedHub.value, wardIds).then(
      x => {
        if (!super.isValidResponse(x)) return;
        this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      }
    );
  }

  isValidateDistricts(): boolean {
    if (!this.selectedHub) {
      this.msgService.error('Vui lòng chọn trạm');
      return true;
    }
    if (!this.selectedDistricts) {
      this.msgService.error('Vui lòng chọn quận huyện');
      return true;
    }
    return false;
  }

  isValidateProvinces(): boolean {
    if (!this.selectedHub) {
      this.msgService.error('Vui lòng chọn trạm');
      return true;
    }
    if (!this.selectedProvinces) {
      this.msgService.error('Vui lòng chọn tỉnh thành');
      return true;
    }
    return false;
  }

}
