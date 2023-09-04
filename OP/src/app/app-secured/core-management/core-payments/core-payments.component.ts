import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PaymentType } from 'src/app/shared/models/entity/paymentType.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PaymentTypeService } from 'src/app/shared/services/api/paymentType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdatePaymentComponent } from '../core-dialog-management/create-update-payment/create-update-payment.component';

@Component({
  selector: 'app-core-payments',
  templateUrl: './core-payments.component.html',
  styleUrls: ['./core-payments.component.scss']
})
export class CorePaymentsComponent extends BaseComponent implements OnInit {
  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  payment: PaymentType[]=[];
  selectedData: any;
  onPageChangeEvent: any;

  constructor(
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    protected paymentTypeService: PaymentTypeService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Hình thức thanh toán' }
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
    this.loadPaymentType();
  }

  async loadPaymentType(): Promise<any> {
    const results = await this.paymentTypeService.getListPaymentType(this.filterViewModel);
    if (results.data.length > 0) {
      this.payment = results.data;
      this.totalRecords = this.payment[0].totalCount || 0;
    } else {
      this.payment = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPaymentType();
   }
   
  refresher(){
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.payment = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadPaymentType();
  }

  createPayment(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdatePaymentComponent, {
      header: `${item ? 'SỬA HÌNH THỨC THANH TOÁN' : 'TẠO MỚI HÌNH THỨC THANH TOÁN'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadPaymentType();
      }
    });
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPaymentType();
  }

  async deletePaymentType(): Promise<any> {
    const res = await this.paymentTypeService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPaymentType();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }
}
