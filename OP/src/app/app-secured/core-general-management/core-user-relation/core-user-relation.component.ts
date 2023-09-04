import { resolveSanitizationFn } from '@angular/compiler/src/render3/view/template';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { readdir } from 'fs';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Relation } from 'src/app/shared/models/entity/Relation.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { User } from 'src/app/shared/models/entity/user.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { UserRelationService } from 'src/app/shared/services/api/userRelation.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';

@Component({
  selector: 'app-core-user-relation',
  templateUrl: './core-user-relation.component.html',
  styleUrls: ['./core-user-relation.component.scss']
})
export class CoreUserRelationComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dataRelation: Relation[] = [];
  dialogDelete = false;
  userRelations: SelectModel[] = [];
  selectUserRelation: SelectModel;
  filterUserRelation: FilterViewModel;
  totalRecords: number;
  relation: Relation = new Relation();
  //user: User = new User();
  selectedData: any;
  filterSearchViewModel: FilterViewModel = {
    searchText: '',
    pageSize: 20,
    pageNumber: 1,
  };
  setTimer: any;
  selectUser: any;
  selectRelation: any;
  users: any[] = [];
  filteredRelation: any[] = [];
  filteredUsers: any[] = [];

  constructor(
    protected breadcrumbService: BreadcrumbService,
    protected authService: AuthService,
    protected userRelationService: UserRelationService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý nhân viên' },
      { label: 'Nhóm nhân viên' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  intData() {
    this.totalRecords = 0;
    this.filterUserRelation = {
      pageNumber: 1,
      pageSize: 20
    };
    // this.loadUser();
  }

  // async loadUser() {
  //   this.users = await this.authService.getUserByTypeUserAsync();
  //   this.first = -1;
  // }

  async loadUserRelationTable() {
    let res = await this.userRelationService.getUserRelation(this.filterUserRelation)
    if (res.data.length > 0) {
      this.dataRelation = res.data;
      this.totalRecords = res.data[0].totalCount;
    } else {
      this.dataRelation = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onPageChange(event: any): void {
    this.first = 0;
    this.filterUserRelation.pageNumber = event.first / event.rows + 1;
    this.filterUserRelation.pageSize = event.rows;
    this.loadUserRelationTable();
  }

  // async onChangeUser() {
  //   let userData = [];
  //   this.filterUserRelation.userId = this.selectUser.value;
  //   this.relation.userId = this.selectUser.value;
  //   if (this.selectUser.value > 0) {
  //     userData = this.users.filter(x => x.value != this.selectUser.value);
  //   }
  //   this.userRelations = userData;
  //   await this.loadUserRelationTable();
  // }

  async onChangeRelation() {
    let relation = await this.authService.getInfoUserById(this.selectUserRelation.value);
    this.relation.userRelationId = this.selectUserRelation.value;
    this.relation.code = relation.data.code;
    this.relation.name = relation.data.userName;
  }

  async save() {
    if (this.isValidate()) { return; }
    let res = await this.userRelationService.createUserRelation(this.relation);
    if (res.data != null) {
      this.msgService.success('Tạo nhóm nhân viên thành công');
      // this.onChangeUser();
      this.loadUserRelationTable();
    } else {
      this.msgService.error(res.message || 'Tạo nhóm nhân viên không thành công!');
    }
  }

  async updatePermission() {
    const res = await this.userRelationService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadUserRelationTable();
      // await this.onChangeUser();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  isValidate(): boolean {
    if (!this.relation.userId) {
      this.msgService.error('Vui lòng chọn nhân viên');
      return true;
    }
    if (!this.relation.userRelationId) {
      this.msgService.error('Vui lòng chọn nhân viên cấp dưới');
      return true;
    }
    return false;
  }

  async filterAutocomplete(event): Promise<any> {
    let filtered: any[] = [];
    const query = event.query;
    this.filterSearchViewModel.searchText = event.query.trim();
    const rep = await this.authService.getUsersBySearch(this.filterSearchViewModel);
    if (rep.isSuccess) {
      filtered = rep.data;
    }
    return filtered;
  }

  async filterUser(event): Promise<any> {
    const filtered: any[] = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.users = await this.filterAutocomplete(event);
        for (const item of this.users) {
          filtered.push(item.fullName);
        }
        this.filteredUsers = filtered;
      } else {
        this.filteredUsers = [];
      }
    }, 1000);
  }

  async filterRelation(event): Promise<any> {
    const filtered: any[] = [];
    let userData = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.users = await this.filterAutocomplete(event);
        if (this.users.length > 0) {
          userData = this.users.filter(x => x.fullName != this.selectUser);
        }
        for (const item of userData) {
          filtered.push(item.fullName);
        }
        this.filteredRelation = filtered;
      } else {
        this.filteredRelation = [];
      }
    }, 1000);
  }

  async onSelectUser(): Promise<void> {
    let findUser = this.users.find(f => f.fullName == this.selectUser);
    if (findUser) {
      this.relation.userId = findUser.id;
      this.filterUserRelation.userId = findUser.id;
      this.loadUserRelationTable();
    }
  }

  onSelectRelation(): void {
    let findRelation = this.users.find(f => f.fullName == this.selectRelation);
    if (findRelation) {
      this.relation.userRelationId = findRelation.id;
      this.relation.code = this.selectRelation;
      this.relation.name = this.selectRelation;
    }
  }
}
