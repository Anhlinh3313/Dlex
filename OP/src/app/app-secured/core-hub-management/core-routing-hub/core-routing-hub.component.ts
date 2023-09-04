import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Table } from 'primeng/table';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { HubRouting } from 'src/app/shared/models/entity/hubRouting.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { HubRoutingService } from 'src/app/shared/services/api/hubRouting.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateRoutingHubComponent } from '../../dialog-genaral/create-update-routing-hub/create-update-routing-hub.component';

@Component({
  selector: 'app-core-routing-hub',
  templateUrl: './core-routing-hub.component.html',
  styleUrls: ['./core-routing-hub.component.scss']
})
export class CoreRoutingHubComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  hubRouting: HubRouting[] = [];
  cloneHubRouting: HubRouting[] = [];
  hubRoutingColone: HubRouting[] = [];
  totalRecords: number;
  dialogDelete: boolean = false;
  filterPoHub: FilterViewModel;
  routinHubLoading: boolean = false;
  ref: DynamicDialogRef;
  selectedData: any;
  centerHubs: SelectModel[] = [];
  selectedCenterHub: SelectModel;
  poHubs: SelectModel[] = [];
  centerHubId: any;
  selectedPoHub: SelectModel;
  hubs: SelectModel[] = [];
  selectedHub: SelectModel;
  stationHubId: number;
  pageNumber = 1;
  pageSize = 20;
  constructor(
    protected breadcrumbService: BreadcrumbService,
    protected hubService: HubService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    private hubRoutingService: HubRoutingService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Phân tuyến giao nhận' }
    ]);
  }
  @ViewChild("dt") dt: Table;

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterPoHub = {
      pageNumber: 1,
      pageSize: 20,
      searchText: ''
    };
    this.getCenterHub();
  }

  async getCenterHub(): Promise<any> {
    this.centerHubs = await this.hubService.getCenterHubAsync();
  }

  async changeCenterHub(): Promise<void> {
    this.centerHubId = this.selectedCenterHub.value;
    this.poHubs = await this.hubService.getSelectModelPoHubByCenterIdbAsync(this.centerHubId);
    this.hubs = null;
    this.hubRouting = [];
  }

  async changePoHub() {
    this.centerHubId = this.selectedPoHub.value;
    await this.loadHub();
    this.stationHubId = this.selectedPoHub.value;
    let data = this.cloneHubRouting.filter(f => f.hub.id === this.selectedPoHub.value);
    if (data.length > 0) {
      this.hubRouting = data;
    }
    await this.loadHubRouting();
    this.selectedHub = null;
  }

  async loadHub(){
    this.hubs = await this.hubService.getSelectModelAsync(this.centerHubId);
  }

  async loadHubRouting() {
    let res = await this.hubRoutingService.getHubRoutingByPoHubId(this.stationHubId)
    if (res.data.length > 0) {
      this.hubRouting = res.data;
      this.cloneHubRouting = JSON.parse(JSON.stringify(this.hubRouting));
      this.totalRecords = res.data.length;
    } else {
      this.hubRouting = [];
      this.totalRecords = 0;
    }
  }

  async changeHub() {
    this.stationHubId = this.selectedHub.value;
    let data = this.cloneHubRouting.filter(f => f.hub.id === this.selectedHub.value);
    if (data.length > 0) {
      this.hubRouting = data;
    }
    await this.loadHubRouting();
  }

  onPageChange(event: any): void {
    this.first = 0;
    this.filterPoHub.pageNumber = event.first / event.rows + 1;
    this.filterPoHub.pageSize = event.rows;
    this.loadHubRouting();
  }

  createOrUpdateUser(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateRoutingHubComponent, {
      header: `${item ? 'SỬA PHÂN TUYẾN GIAO NHẬN' : 'TẠO PHÂN TUYẾN GIAO NHẬN'}`,
      width: '60%',
      style: {'max-height': '90%', 'height': '70%'},
      contentStyle: { 'height': '100%', overflow: 'auto' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: item,
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadHubRouting();
      }
    });
  }

  async deleteRoutingHub(): Promise<any> {
    const res = await this.hubRoutingService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadHubRouting();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  async refresher() {
    this.first = -1;
    this.selectedCenterHub = null;
    this.poHubs = [];
    this.hubs = [];
    this.centerHubId = null;
    await this.getCenterHub();
    await this.loadHubRouting();
  }

}
