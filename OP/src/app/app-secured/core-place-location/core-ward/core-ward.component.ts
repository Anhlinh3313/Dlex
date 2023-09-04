import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { Ward } from 'src/app/shared/models/entity/ward.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateWardComponent } from '../core-dialog/create-update-ward/create-update-ward.component';

@Component({
  selector: 'app-core-ward',
  templateUrl: './core-ward.component.html',
  styleUrls: ['./core-ward.component.scss']
})
export class CoreWardComponent extends BaseComponent implements OnInit {
  //
  districts: Ward[] = [];
  filterViewModel: FilterViewModel;
  selectedData: Ward;
  //
  rows = 20;
  first = 0;
  totalRecords = 0;
  roleLoading = false;
  dialogDelete = false;
  //
  ref: DynamicDialogRef;
  province: SelectModel[] = [];
  district: SelectModel[] = [];
  selectedProvince: SelectModel;
  selectedDistrict: SelectModel;


  constructor(
    protected msgService: MsgService,
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    //
    protected countryService: CountryService,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected wardService: WardService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý địa danh' },
      { label: 'Quản lý phường xã' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData(){
    this.filterViewModel = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.getProvince();
    this.getWards();
  }

  onPageChange(event): void {
    this.first = 0;
    this.filterViewModel.pageNumber = event.first / event.rows + 1;
    this.filterViewModel.pageSize = event.rows;
    this.getWards();
  }

  onFilter(): void {
    this.filterViewModel.pageNumber = 1;
    this.first = 0;
    this.filterViewModel.searchText = this.filterViewModel.searchText.trim();
    this.getWards();
  }

  async getWards(): Promise<any> {
    const results = await this.wardService.getWards(this.filterViewModel);
    if (results.data.length > 0) {
      this.districts = results.data;
      this.totalRecords = this.districts[0].totalCount || 0;
    } else {
      this.districts = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  createOrUpdateWard(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateWardComponent, {
      header: `${item ? 'SỬA PHƯỜNG XÃ' : 'TẠO MỚI PHƯỜNG XÃ'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-role',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.getWards();
      }
    });
  }

  async deleteWard(): Promise<any> {
    const res = await this.wardService.updateWard(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getWards();
    } else {
      this.msgService.error(res.data[0].message  || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher(){
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.filterViewModel.provinceId = null;
    this.filterViewModel.districtid = null;
    this.selectedProvince = null;
    this.selectedDistrict = null;
    this.district = null;
    this.getWards();
  }

  async getProvince(): Promise<any> {
    this.province = await this.provinceService.getProvinceAsync();
  }

  async getDistrict(): Promise<any> {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 1000;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    if(this.selectedProvince.value){
      this.district = await this.districtService.getDistrictAsync(this.filterViewModel);
    } else {
      this.district = [];
    }
  }

  async changeProvince(){
    this.first = 0;
    this.filterViewModel.searchText = null;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    this.filterViewModel.districtid = null;
    await this.getWards();
    await this.getDistrict();
  }

  onChangeDistrict(){
    this.first = 0;
    this.filterViewModel.searchText = null;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.districtid = this.selectedDistrict.value;
    this.getWards();
  }
}
