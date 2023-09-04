import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { ComplainType } from 'src/app/shared/models/entity/complainType.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { ComplainTypeService } from 'src/app/shared/services/api/complainType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateComlaintypeComponent } from '../core-dialog-management/create-update-comlaintype/create-update-comlaintype.component';

@Component({
  selector: 'app-core-type-of-complain',
  templateUrl: './core-type-of-complain.component.html',
  styleUrls: ['./core-type-of-complain.component.scss']
})
export class CoreTypeOfComplainComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  complainType: ComplainType[] = [];

  constructor(
    protected complainTypeService: ComplainTypeService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Loại khiếu nại' }
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
    this.loadComplainType();
  }

  async loadComplainType() {
    const results = await this.complainTypeService.getListComplainType(this.filterViewModel);
    if (results.data.length > 0) {
      this.complainType = results.data;
      this.totalRecords = this.complainType[0].totalCount || 0;
    } else {
      this.complainType = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadComplainType();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.complainType = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadComplainType();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadComplainType();
  }

  async deleteComplainType(): Promise<any> {
    const res = await this.complainTypeService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadComplainType();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }
  
  createComplainType(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateComlaintypeComponent, {
      header: `${item ? 'SỬA LOẠI KHIẾU NẠI' : 'TẠO MỚI LOẠI KHIẾU NẠI'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadComplainType();
      }
    });
  }

}
