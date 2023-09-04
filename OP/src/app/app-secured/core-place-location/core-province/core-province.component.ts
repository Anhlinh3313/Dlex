import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Province } from 'src/app/shared/models/entity/province.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateProvinceComponent } from '../core-dialog/create-update-province/create-update-province.component';

@Component({
  selector: 'app-core-province',
  templateUrl: './core-province.component.html',
  styleUrls: ['./core-province.component.scss']
})
export class CoreProvinceComponent extends BaseComponent implements OnInit {
  //
  provinces: Province[] = [];
  filterViewModel: FilterViewModel;
  selectedData: any;
  //
  rows = 20;
  first = 0;
  totalRecords = 0;
  roleLoading = false;
  dialogDelete = false;
  //
  ref: DynamicDialogRef;
  country: SelectModel[] = [];
  selectedCountry: SelectModel;

  constructor(
    protected msgService: MsgService,
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    //
    protected countryService: CountryService,
    protected provinceService: ProvinceService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý địa danh' },
      { label: 'Quản lý tỉnh thành' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterViewModel = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.getProvinces();
    this.getCountry();
  }

  onPageChange(event): void {
    this.first = 0;
    this.filterViewModel.pageNumber = event.first / event.rows + 1;
    this.filterViewModel.pageSize = event.rows;
    this.getProvinces();
  }

  onFilter(): void {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.filterViewModel.searchText.trim();
    this.getProvinces();
  }

  async getProvinces(): Promise<any> {
    const results = await this.provinceService.getProvinces(this.filterViewModel);
    if (results.data.length > 0) {
      this.provinces = results.data;
      this.totalRecords = this.provinces[0].totalCount || 0;
    } else {
      this.provinces = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  createOrUpdateProvince(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateProvinceComponent, {
      header: `${item ? 'SỬA TỈNH THÀNH' : 'TẠO MỚI TỈNH THÀNH'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-role',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.getProvinces();
      }
    });
  }

  async deleteProvince(): Promise<any> {
    const res = await this.provinceService.updateProvince(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getProvinces();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.filterViewModel.countryId = null;
    this.selectedCountry = null;
    this.getProvinces();
  }

  async getCountry(): Promise<any> {
    this.country = await this.countryService.getCountryAsync();
  }

  changeCountry(){
    this.first = 0;
    this.filterViewModel.searchText = null;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.countryId = this.selectedCountry.value;
    this.getProvinces();
  }
}
