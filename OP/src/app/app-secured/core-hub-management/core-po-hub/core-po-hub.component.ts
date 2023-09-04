import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Hub } from 'src/app/shared/models/entity/Hub.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdatePoHubComponent } from '../../dialog-genaral/create-update-po-hub/create-update-po-hub.component';

@Component({
  selector: 'app-core-po-hub',
  templateUrl: './core-po-hub.component.html',
  styleUrls: ['./core-po-hub.component.scss']
})
export class CorePoHubComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  poHub: Hub[] = [];
  totalRecords: number;
  searchText: string;
  dialogDelete: boolean = false;
  filterPoHub: FilterViewModel;
  poHubLoading: boolean = false;
  ref: DynamicDialogRef;
  selectedData: any;
  centerHubs: SelectModel[] = [];
  selectedCenterHub: SelectModel;

  constructor(
    protected breadcrumbService: BreadcrumbService,
    protected hubService: HubService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Quản lý chi nhánh' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterPoHub = {
      pageNumber: 1,
      pageSize: 20,
      searchText: ''
    };
    this.loadPoHub();
    this.getCenterHub();
  }

  async loadPoHub() {
    let res = await this.hubService.getPoHubs(this.filterPoHub)
    if (res.data.length > 0) {
      this.poHub = res.data
      this.totalRecords = res.data[0].totalCount;
    } else {
      this.poHub = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  search() {
    this.filterPoHub.pageNumber = 1;
    this.filterPoHub.pageSize = 20;
    this.first = 0;
    this.filterPoHub.searchText = this.searchText.trim();
    this.loadPoHub();
  }

  onPageChange(event: any): void {
    this.first = 0;
    this.filterPoHub.pageNumber = event.first / event.rows + 1;
    this.filterPoHub.pageSize = event.rows;
    this.loadPoHub();
  }

  createOrUpdateUser(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdatePoHubComponent, {
      header: `${item ? 'SỬA CHI NHÁNH' : 'TẠO MỚI CHI NHÁNH'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadPoHub();
      }
    });
  }

  async deletePoHub(): Promise<any> {
    const res = await this.hubService.updatePoHub(this.selectedData.id);
    if (res.data[0].isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPoHub();
    } else {
      this.msgService.error(res.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  refresher() {
    this.searchText = null
    this.filterPoHub.pageNumber = 1;
    this.filterPoHub.pageSize = 20;
    this.filterPoHub.searchText = null;
    this.filterPoHub.centerHubId = null;
    this.selectedCenterHub = null;
    this.loadPoHub();
  }

  async getCenterHub(): Promise<any> {
    this.centerHubs = await this.hubService.getCenterHubAsync();
  }

  changeCenterHub(){
    this.first = 0;
    this.filterPoHub.searchText = null;
    this.filterPoHub.pageNumber = 1;
    this.filterPoHub.pageSize = 20;
    this.filterPoHub.centerHubId = this.selectedCenterHub.value;
    this.loadPoHub();
  }
}
