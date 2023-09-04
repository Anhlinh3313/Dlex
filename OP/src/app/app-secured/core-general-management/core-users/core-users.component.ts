import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Role } from 'src/app/shared/models/entity/role.model';
import { User } from 'src/app/shared/models/entity/user.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateUserComponent } from '../core-dialog/create-update-user/create-update-user.component';

@Component({
  selector: 'app-core-users',
  templateUrl: './core-users.component.html',
  styleUrls: ['./core-users.component.scss']
})
export class CoreUsersComponent extends BaseComponent implements OnInit {
  //
  users: User[] = [];
  //
  rows = 20;
  first = 0;
  totalRecords: number;
  usersLoading = false;
  dialogDelete = false;
  ref: DynamicDialogRef;
  searchText: string;
  filterUser: FilterViewModel;
  onPageChangeEvent: any;
  selectedData: any;
  //

  constructor(
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected dialogService: DialogService,
    private authService: AuthService,
    protected router: Router,
    protected permissionService: PermissionService,
    //
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý hệ thống' },
      { label: 'Tài khoản' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.filterUser = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.loadDataUser();
  }

  onClick(): void {
    this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
  }

  createOrUpdateUser(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateUserComponent, {
      header: `${item ? 'SỬA TÀI KHOẢN' : 'TẠO MỚI TÀI KHOẢN'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadDataUser();
      }
    });
  }

  async deleteUser(): Promise<any> {
    const res = await this.authService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadDataUser();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  async loadDataUser() {
    this.filterUser.cols = 'Department,Role,Hub,ManageHub';
    const res = await this.authService.search(this.filterUser);
    if (res.data.length > 0) {
      this.users = res.data;
      this.totalRecords = res.dataCount;
    } else {
      this.users = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  showRole(role: Role[]) {
    let roleNames = '';
    role.map(x => roleNames += x.name + ', ');
    return roleNames;
  }

  search() {
    this.filterUser.pageNumber = 1;
    this.first = 0;
    this.filterUser.searchText = this.searchText.trim();
    this.loadDataUser();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterUser.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterUser.pageSize = this.onPageChangeEvent.rows;
    this.loadDataUser();
  }
  refresher(): void {
    this.filterUser.pageNumber = 1;
    this.filterUser.pageSize = 20;
    this.filterUser.searchText = '';
    this.users = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadDataUser();
  }
}
