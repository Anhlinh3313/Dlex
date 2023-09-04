import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PriceListSetting } from 'src/app/shared/models/entity/priceListSetting.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CustomerService } from 'src/app/shared/services/api/customer.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PriceListService } from 'src/app/shared/services/api/priceList.service';
import { PriceListSettingService } from 'src/app/shared/services/api/priceListSetting.service';
import { ServiceService } from 'src/app/shared/services/api/service.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-price-list-setting',
  templateUrl: './core-price-list-setting.component.html',
  styleUrls: ['./core-price-list-setting.component.scss']
})
export class CorePriceListSettingComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  customer: SelectModel[] = [];
  service: SelectModel[] = [];
  priceList: SelectModel[] = [];
  selectCustomer: SelectModel;
  selectService: SelectModel;
  selectPriceList: SelectModel;
  priceListSetting: PriceListSetting[] = [];
  dataPriceListSetting: PriceListSetting = new PriceListSetting();
  vatSurcharge: any;
  fuelSurcharge: any;
  vsvxSurcharge: any;
  dim: any;
  checkCreate: boolean = true;

  constructor(
    protected priceListSettingService: PriceListSettingService,
    protected priceListService: PriceListService,
    protected serviceService: ServiceService,
    protected customerService: CustomerService,
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý khách hàng' },
      { label: 'Cài đặt bảng giá' }
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
    this.loadCustomer();
    this.loadService();
    this.laodPriceList();
    this.loadPriceListSetting();
  }

  async loadPriceListSetting() {
    const results = await this.priceListSettingService.getListPriceListSetting(this.filterViewModel);
    if (results.data.length > 0) {
      this.priceListSetting = results.data;
      this.totalRecords = this.priceListSetting[0].totalCount || 0;
    } else {
      this.priceListSetting = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  async loadCustomer() {
    this.customer = await this.customerService.getAllCustomerAsync();
  }

  async loadService() {
    this.service = await this.serviceService.getAllServiceAsync();
  }

  async laodPriceList() {
    this.priceList = await this.priceListService.getAllPriceListAsync();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPriceListSetting();
  }

  refresher() {
    this.checkCreate = true;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.customerId = null;
    this.filterViewModel.serviceId = null;
    this.filterViewModel.priceListId = null;
    this.selectCustomer = null;
    this.selectService = null;
    this.selectPriceList = null;
    this.vatSurcharge = null;
    this.fuelSurcharge = null;
    this.vsvxSurcharge = null;
    this.dim = null;
  }

  changeCustomer() {
    this.filterViewModel.customerId = this.selectCustomer.value;
    this.dataPriceListSetting.customerId = this.selectCustomer.value;
    this.loadPriceListSetting();
  }

  changeService() {
    this.filterViewModel.serviceId = this.selectService.value;
    this.dataPriceListSetting.serviceId = this.selectService.value;
    this.loadPriceListSetting();
  }

  changePriceList() {
    this.filterViewModel.priceListId = this.selectPriceList.value;
    this.dataPriceListSetting.priceListId = this.selectPriceList.value;
    this.loadPriceListSetting();
  }

  async createOrUpdateUser(item: any = null): Promise<void> {
    this.checkCreate = false;
    this.dataPriceListSetting = item;
    this.vatSurcharge = item.vatSurcharge;
    this.fuelSurcharge = item.fuelSurcharge;
    this.vsvxSurcharge = item.vsvxSurcharge;
    this.dim = item.dim;
    this.loadUpdateCustomer();
    this.loadUpdateService();
    this.loadUpdatePriceList();
  }

  loadUpdateCustomer() {
    const findCustomer = this.customer.find(f => f.value === this.dataPriceListSetting.customerId);
    if (findCustomer) {
      this.selectCustomer = findCustomer;
    }
  }

  loadUpdateService() {
    const findService = this.service.find(f => f.value === this.dataPriceListSetting.serviceId);
    if (findService) {
      this.selectService = findService;
    }
  }

  loadUpdatePriceList() {
    const findPriceList = this.priceList.find(f => f.value === this.dataPriceListSetting.priceListId);
    if (findPriceList) {
      this.selectPriceList = findPriceList;
    }
  }

  async createPriceListSetting() {
    if (!this.isValidData()) { return; }
    this.dataPriceListSetting.vatSurcharge = this.vatSurcharge;
    this.dataPriceListSetting.fuelSurcharge = this.fuelSurcharge;
    this.dataPriceListSetting.vsvxSurcharge = this.vsvxSurcharge;
    this.dataPriceListSetting.dim = this.dim;
    let code = this.selectCustomer.data.code + this.selectPriceList.data.code;
    this.dataPriceListSetting.code = code;
    this.dataPriceListSetting.name = code;
    let res = await this.priceListSettingService.create(this.dataPriceListSetting)
    if (res.isSuccess) {
      this.msgService.success('Tạo Cài đặt bảng giá thành công');
      this.refresher();
      this.loadPriceListSetting();
    } else {
      let a = Object.values(res.data)
      if (a) {
        this.msgService.error('Cài đặt bảng giá đã tồn tại');
      } else {
        this.msgService.error('Cài đặt bảng giá thành công không thành công');
      }
    }
  }

  async updatePriceListSetting() {
    if (!this.isValidData()) { return; }
    this.dataPriceListSetting.vatSurcharge = this.vatSurcharge;
    this.dataPriceListSetting.fuelSurcharge = this.fuelSurcharge;
    this.dataPriceListSetting.vsvxSurcharge = this.vsvxSurcharge;
    this.dataPriceListSetting.dim = this.dim;
    let code = this.selectCustomer.data.code + this.selectPriceList.data.code;
    this.dataPriceListSetting.code = code;
    this.dataPriceListSetting.name = code;
    this.dataPriceListSetting.isEnabled = true;
    let res = await this.priceListSettingService.update(this.dataPriceListSetting)
    if (res.isSuccess) {
      this.msgService.success('Cập nhật cài đặt bảng giá thành công');
      this.refresher();
      this.loadPriceListSetting();
    } else {
      let a = Object.values(res.data)
      if (a) {
        this.msgService.error('Cài đặt bảng giá đã tồn tại');
      } else {
        this.msgService.error(a.toString() || 'Cập nhật cài đặt bảng giá không thành công!');
      }
    }
  }

  async deletePriceListSetting(): Promise<any> {
    const res = await this.priceListSettingService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPriceListSetting();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }


  isValidData(): boolean {

    if (!this.selectCustomer) {
      this.msgService.error('Vui lòng chọn khách hàng');
      return false;
    }

    if (!this.selectService) {
      this.msgService.error('Vui lòng chọn dịch vụ');
      return false;
    }

    if (!this.selectPriceList) {
      this.msgService.error('Vui lòng chọn mã bảng giá');
      return false;
    }
    return true;
  }
}
