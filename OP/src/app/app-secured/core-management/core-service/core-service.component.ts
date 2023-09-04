import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Service } from 'src/app/shared/models/entity/service.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ServiceService } from 'src/app/shared/services/api/service.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateServiceComponent } from '../core-dialog-management/create-update-service/create-update-service.component';

@Component({
  selector: 'app-core-service',
  templateUrl: './core-service.component.html',
  styleUrls: ['./core-service.component.scss']
})
export class CoreServiceComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  service: Service[] = [];

  constructor(
    protected serviceService: ServiceService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Dịch vụ' }
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
    this.loadService();
  }

  async loadService(): Promise<any> {
    const results = await this.serviceService.getListService(this.filterViewModel);
    if (results.data.length > 0) {
      this.service = results.data;
      this.totalRecords = this.service[0].totalCount || 0;
    } else {
      this.service = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadService();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.service = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadService();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadService();
  }


  createService(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateServiceComponent, {
      header: `${item ? 'SỬA DỊCH VỤ' : 'TẠO MỚI DỊCH VỤ'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadService();
      }
    });
  }

  async deleteService(): Promise<any> {
    const res = await this.serviceService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadService();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }
}