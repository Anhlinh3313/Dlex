import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Role } from 'src/app/shared/models/entity/role.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateRoleComponent } from '../core-dialog/create-update-role/create-update-role.component';

@Component({
  selector: 'app-core-roles',
  templateUrl: './core-roles.component.html',
  styleUrls: ['./core-roles.component.scss']
})
export class CoreRolesComponent extends BaseComponent implements OnInit {
  //
  roles: Role[] = [];
  filterUser: FilterViewModel;
  selectedData: any;
  //
  rows = 20;
  first = 0;
  totalRecords = 0;
  roleLoading = false;
  dialogDelete = false;
  //
  ref: DynamicDialogRef;

  constructor(
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService, 
    protected roleService: RoleService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Chức vụ' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData(){
    this.filterUser = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.getRole();
  }

  async getRole(): Promise<any> {
    const results = await this.roleService.getRole(this.filterUser);
    if (results.data.length > 0) {
      this.roles = results.data;
      this.totalRecords = this.roles[0].totalCount;
    } else {
      this.roles = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onPageChange(event): void {
    this.filterUser.pageNumber = event.first / event.rows + 1;
    this.filterUser.pageSize = event.rows;
    this.getRole();
  }

  onFilter(): void {
    this.filterUser.pageNumber = 1;
    this.first = 0;
    this.getRole();
  }

  createOrUpdateRole(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateRoleComponent, {
      header: `${item ? 'SỬA CHỨC VỤ' : 'TẠO MỚI CHỨC VỤ'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-role',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.getRole();
      }
    });
  }

  async deleteRole(): Promise<any> {
    if (this.isValidate()) { return; }
    const res = await this.roleService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.getRole();
    } else {
      this.msgService.msgBoxError(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  isValidate(): boolean {
    if (this.selectedData.id == 6 || this.selectedData.id == 9 || this.selectedData.id == 31) {
      this.msgService.msgBoxError('Chức vụ không được phéo xoá');
      return true;
    }
    return false;
  }

  refresher(){
    this.filterUser.searchText = null;
    this.filterUser.pageNumber = 1;
    this.filterUser.pageSize = 20;
    this.getRole();
  }
}
