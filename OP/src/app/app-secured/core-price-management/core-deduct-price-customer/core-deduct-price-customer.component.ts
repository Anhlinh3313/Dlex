import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PromotionCustomer } from 'src/app/shared/models/entity/promotionCustomer.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CustomerService } from 'src/app/shared/services/api/customer.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PromotionService } from 'src/app/shared/services/api/promotion.service';
import { PromotionCustomerService } from 'src/app/shared/services/api/promotionCustomer.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-deduct-price-customer',
  templateUrl: './core-deduct-price-customer.component.html',
  styleUrls: ['./core-deduct-price-customer.component.scss']
})
export class CoreDeductPriceCustomerComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  promotion: SelectModel[] = [];
  selectPromotion: any = null;
  customer: SelectModel[] = [];
  selectCustomer: any = null;
  promotionCustomer: PromotionCustomer[] = [];
  createSelectPromotion: any = null;
  createSelectCustomer: any = null;

  constructor(
    protected customerService: CustomerService,
    protected promotionCustomerService: PromotionCustomerService,
    protected promotionService: PromotionService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý bảng giá' },
      { label: 'Áp giảm giá khách hàng' }
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
    this.loadPromotion();
    this.loadPromotionCustomer();
  }

  async loadPromotion() {
    this.promotion = await this.promotionService.getAllPromotionAsync();
  }

  async loadCustomer() {
    this.customer = await this.customerService.getAllCustomerAsync();
  }

  async loadPromotionCustomer() {
    const results = await this.promotionCustomerService.getListPromotionCustomer(this.filterViewModel);
    if (results.data.length > 0) {
      this.promotionCustomer = results.data;
      this.totalRecords = this.promotionCustomer[0].totalCount || 0;
    } else {
      this.promotionCustomer = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPromotionCustomer();
  }

  refresher(): void {
    this.selectPromotion = null;
    this.selectCustomer = null;
    this.createSelectPromotion = null;
    this.createSelectCustomer = null;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.promotionCustomer = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadPromotionCustomer();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPromotionCustomer();
  }

  changePromotion() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = '';
    this.filterViewModel.promotionId = this.selectPromotion.value;
    this.filterViewModel.customerId = null;
    this.loadPromotionCustomer();
  }

  changeCustomer() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = '';
    this.filterViewModel.promotionId = null;
    this.filterViewModel.customerId = this.selectCustomer.value;
    this.loadPromotionCustomer();
  }


  changePromotionCreate() {
    let promotionCustomers = new PromotionCustomer();
    promotionCustomers.promotionId = this.createSelectPromotion.value;
  }

  changeCustomerCreate() {
    let promotionCustomers = new PromotionCustomer();
    promotionCustomers.customerId = this.createSelectCustomer.value;
  }

  async createPromotionCustomer() {
    if (!this.isValidData()) { return; }
    let promotionCustomers = new PromotionCustomer();
    promotionCustomers.promotionId = this.createSelectPromotion.value;
    promotionCustomers.customerId = this.createSelectCustomer.value;
    let res = await this.promotionCustomerService.createPromotionCustomer(promotionCustomers.customerId, promotionCustomers.promotionId)
    if (res.data[0].isSuccess) {
      this.msgService.success('Tạo áp giảm giá khách hàng');
      this.loadPromotionCustomer();
    } else {
      this.msgService.error(res.data[0].message);
    }
  }

  isValidData(): boolean {

    if (!this.createSelectPromotion) {
      this.msgService.error('Vui lòng chọn mã giảm giá');
      return false;
    }

    if (!this.createSelectCustomer) {
      this.msgService.error('Vui lòng chọn khách hàng');
      return false;
    }

    return true;
  }

  async deletePromotionCustomer(): Promise<any> {
    const res = await this.promotionCustomerService.updatePromotionCustomer(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPromotionCustomer();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

}
