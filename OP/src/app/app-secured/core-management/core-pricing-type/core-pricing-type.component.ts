import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PricingType } from 'src/app/shared/models/entity/pricingType.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PricingTypeService } from 'src/app/shared/services/api/pricingType.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdatePricingTypeComponent } from '../core-dialog-management/create-update-pricing-type/create-update-pricing-type.component';

@Component({
  selector: 'app-core-pricing-type',
  templateUrl: './core-pricing-type.component.html',
  styleUrls: ['./core-pricing-type.component.scss']
})
export class CorePricingTypeComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  pricingType: PricingType[]=[];
  
  constructor(
    protected pricingTypeService: PricingTypeService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Loại giá' }
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
    this.loadPricingType();
  }

  async loadPricingType(): Promise<any> {
    const results = await this.pricingTypeService.getListPricingType(this.filterViewModel);
    if (results.data.length > 0) {
      this.pricingType = results.data;
      this.totalRecords = this.pricingType[0].totalCount || 0;
    } else {
      this.pricingType = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPricingType();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.pricingType = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadPricingType();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPricingType();
  }

  createPricingType(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdatePricingTypeComponent, {
      header: `${item ? 'SỬA CÔNG THỨC TÍNH GIÁ' : 'TẠO MỚI CÔNG THỨC TÍNH GIÁ'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadPricingType();
      }
    });
  }

  async deleteFormula(): Promise<any> {
    const res = await this.pricingTypeService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPricingType();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }
}
