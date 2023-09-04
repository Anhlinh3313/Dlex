import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Customer } from 'src/app/shared/models/entity/customer.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CustomerService } from 'src/app/shared/services/api/customer.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateOrUpdateCustomerComponent } from '../core-dialog-customer/create-or-update-customer/create-or-update-customer.component';

@Component({
  selector: 'app-core-customer-info',
  templateUrl: './core-customer-info.component.html',
  styleUrls: ['./core-customer-info.component.scss']
})
export class CoreCustomerInfoComponent extends BaseComponent implements OnInit {
  //
  accepts: SelectModel[] = [
    { value: null, data: null, label: 'Chọn tất cả' },
    { value: false, data: null, label: 'Chưa kích hoạt' },
    { value: true, data: null, label: 'Đã kích hoạt' },
  ];
  selectedAccept: SelectModel;
  provinces: SelectModel[] = [];
  selectedProvince: SelectModel;

  searchText = '';
  tableValues: Customer[] = [];
  filterViewModel: FilterViewModel = {
    pageSize: 10,
    pageNumber: 1,
    searchText: null,
    provinceId: null,
    isAccept: null,
    customerId: null,
  };
  selectedData: Customer = new Customer();

  rows = 10;
  first = 0;
  totalRecords = 0;
  lazyLoading = false;
  dialogDelete = false;
  ref: DynamicDialogRef;

  constructor(
    protected messageService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    //
    public customerService: CustomerService,
    public provinceService: ProvinceService,
  ) {
    super(messageService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý khách hàng' },
      { label: 'Thông tin khách hàng' }
    ]);
  }

  ngOnInit(): void {
    this.initData();
  }

  async initData(): Promise<any> {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = this.rows;
    await this.getProvince();
    await this.getCustomer();
  }

  async getCustomer(): Promise<any> {
    this.lazyLoading = true;
    const req = await this.customerService.getCustomerByFilter(this.filterViewModel);
    if (req.data.length > 0) {
      this.tableValues = req.data as Customer[];
      this.totalRecords = this.tableValues[0].totalCount;
      this.lazyLoading = false;
    } else {
      this.tableValues = [];
      this.totalRecords = 0;
      this.first = 0;
      this.lazyLoading = false;
    }
  }
  async createOrUpdate(item): Promise<any> {
    this.ref = this.dialogService.open(CreateOrUpdateCustomerComponent, {
      header: `${item ? 'SỬA THÔNG TIN KHÁCH HÀNG' : 'TẠO MỚI THÔNG TIN KHÁCH HÀNG'}`,
      width: '65%',
      contentStyle: { 'max-height': '80%', overflow: 'auto' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe(async (res: any) => {
      if (res) {
        await this.getCustomer();
      }
    });
  }
  
  async onFilter(): Promise<any> {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.getCustomer();
  }

  async refresher(): Promise<any> {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = this.rows;
    this.filterViewModel.searchText = null;
    this.filterViewModel.provinceId = null;
    this.filterViewModel.isAccept = null;
    this.filterViewModel.customerId = null;
    this.filterViewModel.searchText = null;

    this.lazyLoading = false;
    this.searchText = null;

    this.selectedAccept = null;
    this.selectedProvince = null;
    this.selectedData = null;

    await this.getCustomer();
  }
  async onPageChange(event): Promise<any> {
    this.filterViewModel.pageSize = event.rows;
    this.filterViewModel.pageNumber = event.first / event.rows + 1;
    // this.first = event.first / event.rows + 1;
    await this.getCustomer();
  }

  async getCustomerByAccept(): Promise<any> {
    this.filterViewModel.isAccept = this.selectedAccept.value;
    await this.getCustomer();
  }
  async getCustomerByProvinceId(): Promise<any> {
    this.filterViewModel.provinceId = this.selectedProvince.value;
    await this.getCustomer();
  }
  async getProvince(): Promise<any> {
    this.provinces =  await this.provinceService.getProvinceAsync();
  }
  async deleteCustomer(): Promise<any> {
    await this.customerService.delete(this.selectedData);
    this.dialogDelete = false;
    this.messageService.success('Cập nhật của bạn đã được thay đổi trên hệ thống!');
    this.getCustomer();
  }
}
