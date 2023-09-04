import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { AccountBank } from 'src/app/shared/models/entity/accountBank.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AccountBankService } from 'src/app/shared/services/api/accountBank.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateBankAccountComponent } from '../core-dialog-management/create-update-bank-account/create-update-bank-account.component';

@Component({
  selector: 'app-core-bank-accounts',
  templateUrl: './core-bank-accounts.component.html',
  styleUrls: ['./core-bank-accounts.component.scss']
})
export class CoreBankAccountsComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  accountBank: AccountBank[] = [];

  constructor(
    protected accountBankService: AccountBankService,
    protected dialogService: DialogService,
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Tài khoản ngân hàng' }
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
    this.laodBankAccount();
  }

  async laodBankAccount(): Promise<any> {
    const results = await this.accountBankService.getListBankAccount(this.filterViewModel);
    if (results.data.length > 0) {
      this.accountBank = results.data;
      this.totalRecords = this.accountBank[0].totalCount || 0;
    } else {
      this.accountBank = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.laodBankAccount();
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.laodBankAccount();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.accountBank = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.laodBankAccount();
  }

  async deleteBankAccount(): Promise<any> {
    const res = await this.accountBankService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.laodBankAccount();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  createBankAccount(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateBankAccountComponent, {
      header: `${item ? 'SỬA TÀI KHOẢN NGÂN HÀNG' : 'TẠO MỚI TÀI KHOẢN NGÂN HÀNG'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.laodBankAccount();
      }
    });
  }

}
