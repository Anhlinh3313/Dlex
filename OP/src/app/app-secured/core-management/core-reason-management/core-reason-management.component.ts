import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Reason } from 'src/app/shared/models/entity/reason.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ReasonService } from 'src/app/shared/services/api/reason.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateReasonComponent } from '../core-dialog-management/create-update-reason/create-update-reason.component';

@Component({
  selector: 'app-core-reason-management',
  templateUrl: './core-reason-management.component.html',
  styleUrls: ['./core-reason-management.component.scss']
})
export class CoreReasonManagementComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  reason: Reason[]=[];

  constructor(
    protected reasonService: ReasonService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Quản lý lý do' }
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
    this.loadReason();
  }

  async loadReason(): Promise<any> {
    const results = await this.reasonService.getListReason(this.filterViewModel);
    if (results.data.length > 0) {
      this.reason = results.data;
      this.totalRecords = this.reason[0].totalCount || 0;
    } else {
      this.reason = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadReason();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.reason = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadReason();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadReason();
  }

  async deleteReason(): Promise<any> {
    const res = await this.reasonService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadReason();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  createReason(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateReasonComponent, {
      header: `${item ? 'SỬA LÝ DO' : 'TẠO MỚI LÝ DO'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadReason();
      }
    });
  }
}
