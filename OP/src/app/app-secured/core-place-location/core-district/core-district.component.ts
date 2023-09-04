import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { District } from 'src/app/shared/models/entity/district.model';
import { Province } from 'src/app/shared/models/entity/province.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateDistrictComponent } from '../core-dialog/create-update-district/create-update-district.component';
import { CreateUpdateProvinceComponent } from '../core-dialog/create-update-province/create-update-province.component';

@Component({
  selector: 'app-core-district',
  templateUrl: './core-district.component.html',
  styleUrls: ['./core-district.component.scss']
})
export class CoreDistrictComponent extends BaseComponent implements OnInit {
  //
  districts: District[] = [];
  filterViewModel: FilterViewModel;
  selectedData: District;
  //
  rows = 20;
  first = 0;
  totalRecords = 0;
  roleLoading = false;
  dialogDelete = false;
  //
  ref: DynamicDialogRef;
  province: SelectModel[] = [];
  selectedProvince: SelectModel;


  constructor(
    protected msgService: MsgService,
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    //
    protected countryService: CountryService,
    protected provinceService: ProvinceService,
    protected districtService: DistrictService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý địa danh' },
      { label: 'Quản lý quận huyện' }
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
    this.getDistricts();
    this.getProvince();
  }

  onPageChange(event): void {
    this.first = 0;
    this.filterViewModel.pageNumber = event.first / event.rows + 1;
    this.filterViewModel.pageSize = event.rows;
    this.getDistricts();
  }

  onFilter(): void {
    this.filterViewModel.pageNumber = 1;
    this.first = 0;
    this.filterViewModel.searchText = this.filterViewModel.searchText.trim();
    this.getDistricts();
  }

  async getDistricts(): Promise<any> {
    const results = await this.districtService.getDistricts(this.filterViewModel);
    if (results.data.length > 0) {
      this.districts = results.data;
      this.totalRecords = this.districts[0].totalCount || 0;
    } else {
      this.districts = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  createOrUpdateDistrict(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateDistrictComponent, {
      header: `${item ? 'SỬA QUẬN HUYỆN' : 'TẠO MỚI QUẬN HUYỆN'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-role',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.getDistricts();
      }
    });
  }

  async deleteDistrict(): Promise<any> {
    const res = await this.districtService.updateDistrict(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getDistricts();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;   
    this.filterViewModel.provinceId =  null;
    this.selectedProvince = null;
    this.getDistricts();
  }

  async getProvince(): Promise<any> {
    this.province = await this.provinceService.getProvinceAsync();
  }

  changeProvince(){
    this.first = 0;
    this.filterViewModel.searchText = null;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    this.getDistricts();
  }
}
