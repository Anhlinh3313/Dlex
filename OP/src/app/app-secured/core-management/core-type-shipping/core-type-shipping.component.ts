import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { TransportType } from 'src/app/shared/models/entity/transportType.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { TransportTypeService } from 'src/app/shared/services/api/transportType.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateTypeShippingComponent } from '../core-dialog-management/create-update-type-shipping/create-update-type-shipping.component';

@Component({
  selector: 'app-core-type-shipping',
  templateUrl: './core-type-shipping.component.html',
  styleUrls: ['./core-type-shipping.component.scss']
})
export class CoreTypeShippingComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  transportType: TransportType[]=[];
  
  constructor(
    protected transportTypeService: TransportTypeService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Loại vận chuyển' }
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
    this.loadTransportType();
  }

  async loadTransportType(){
    const results = await this.transportTypeService.getListTransportType(this.filterViewModel);
    if (results.data.length > 0) {
      this.transportType = results.data;
      this.totalRecords = this.transportType[0].totalCount || 0;
    } else {
      this.transportType = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  
  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadTransportType();
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.transportType = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadTransportType();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadTransportType();
  }

  async deleteTransportType() {
    const res = await this.transportTypeService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadTransportType();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  createTransportType(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateTypeShippingComponent, {
      header: `${item ? 'SỬA LOẠI VẬN CHUYỂN' : 'TẠO MỚI LOẠI VẬN CHUYỂN'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadTransportType();
      }
    });
  }


}
