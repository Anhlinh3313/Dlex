import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Structure } from 'src/app/shared/models/entity/structure.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { StructureService } from 'src/app/shared/services/api/Structure.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateTypeCargoComponent } from '../core-dialog-management/create-update-type-cargo/create-update-type-cargo.component';

@Component({
  selector: 'app-core-type-cargo',
  templateUrl: './core-type-cargo.component.html',
  styleUrls: ['./core-type-cargo.component.scss']
})
export class CoreTypeCargoComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  structure: Structure[] = [];

  constructor(
    protected structureService: StructureService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Loại hàng hoá' }
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
    this.loadStructure();
  }

  async loadStructure(): Promise<any> {
    const results = await this.structureService.getListStructure(this.filterViewModel);
    if (results.data.length > 0) {
      this.structure = results.data;
      this.totalRecords = this.structure[0].totalCount || 0;
    } else {
      this.structure = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadStructure();
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.structure = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadStructure();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadStructure();
  }

  async deleteStructure() {
    const res = await this.structureService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadStructure();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  createStructure(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateTypeCargoComponent, {
      header: `${item ? 'SỬA LOẠI HÀNG HOÁ' : 'TẠO MỚI LOẠI HÀNG HOÁ'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadStructure();
      }
    });
  }
}
