import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Hub } from 'src/app/shared/models/entity/Hub.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateHubComponent } from '../../dialog-genaral/create-update-hub/create-update-hub.component';

@Component({
  selector: 'app-core-center-hub',
  templateUrl: './core-center-hub.component.html',
  styleUrls: ['./core-center-hub.component.scss']
})
export class CoreCenterHubComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  ref: DynamicDialogRef;
  searchText: string;
  centerHub: Hub[] = [];
  totalRecords: number;
  dialogDelete = false;
  selectedData: any;
  filterCenterHub: FilterViewModel;
  hubsLoading: boolean = false;

  constructor(
    protected dialogService: DialogService,
    private hubService: HubService,
    protected msgService: MsgService,
    protected breadcrumbService: BreadcrumbService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Quản lý trung tâm' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterCenterHub = {
      pageNumber: 1,
      pageSize: 20,
      searchText: ''
    };
    this.loadCenterHub();
  }

  createOrUpdateUser(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateHubComponent, {
      header: `${item ? 'SỬA TRUNG TÂM' : 'TẠO MỚI TRUNG TÂM'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadCenterHub();
      }
    });
  }

  search() {
    this.filterCenterHub.pageNumber = 1;
    this.filterCenterHub.pageSize = 20;
    this.filterCenterHub.searchText = this.searchText.trim();
    this.loadCenterHub();
  }

  async loadCenterHub() {
    let res = await this.hubService.getCenterHubs(this.filterCenterHub)
    if (res.data.length > 0) {
      this.centerHub = res.data;
      this.totalRecords = res.data[0].totalCount;
    } else {
      this.centerHub = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onPageChange(event: any): void {
    this.first = 0;
    this.filterCenterHub.pageNumber = event.first / event.rows + 1;
    this.filterCenterHub.pageSize = event.rows;
    this.loadCenterHub();
  }


  async deleteCenterHub(): Promise<any> {
    const res = await this.hubService.updateCenterHub(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadCenterHub();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher() {
    this.filterCenterHub.pageNumber = 1;
    this.filterCenterHub.pageSize = 20;
    this.filterCenterHub.searchText = null;
    this.loadCenterHub();
  }
}
