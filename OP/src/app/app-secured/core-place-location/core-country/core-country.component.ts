import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SSL_OP_SSLEAY_080_CLIENT_DH_BUG } from 'constants';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Country } from 'src/app/shared/models/entity/country.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CountryService } from 'src/app/shared/services/api/country.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateCountryComponent } from '../core-dialog/create-update-country/create-update-country.component';

@Component({
  selector: 'app-core-country',
  templateUrl: './core-country.component.html',
  styleUrls: ['./core-country.component.scss']
})
export class CoreCountryComponent extends BaseComponent implements OnInit {
  //
  countries: Country[] = [];
  filterCountry: FilterViewModel;
  selectedData: any;
  //
  rows = 20;
  first = 0;
  totalRecords = 0;
  roleLoading = false;
  dialogDelete = false;
  //
  ref: DynamicDialogRef;

  constructor(
    protected msgService: MsgService,
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    //
    protected countryService: CountryService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý địa danh' },
      { label: 'Quản lý quốc gia' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterCountry = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.getCountry();
  }

  onPageChange(event): void {
    this.first = 0;
    this.filterCountry.pageNumber = event.first / event.rows + 1;
    this.filterCountry.pageSize = event.rows;
    this.getCountry();
  }

  onFilter(): void {
    this.filterCountry.pageNumber = 1;
    this.first = 0;
    this.getCountry();
  }

  async getCountry(): Promise<any> {
    const results = await this.countryService.getListCountrys(this.filterCountry);
    if (results.data.length > 0) {
      this.countries = results.data;
      this.totalRecords = results.dataCount;
    } else {
      this.countries = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  createOrUpdateCountry(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateCountryComponent, {
      header: `${item ? 'SỬA QUỐC GIA' : 'TẠO MỚI QUỐC GIA'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-role',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.getCountry();
      }
    });
  }

  async deleteCountry(): Promise<any> {
    const res = await this.countryService.updateCountry(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getCountry();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher() {
    this.filterCountry.pageNumber = 1;
    this.filterCountry.pageSize = 20;
    this.filterCountry.searchText = null;
    this.getCountry();
  }
}
