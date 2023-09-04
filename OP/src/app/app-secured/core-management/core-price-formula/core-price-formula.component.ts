import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Formula } from 'src/app/shared/models/entity/formula.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { FormulaService } from 'src/app/shared/services/api/formula.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdateFormulaComponent } from '../core-dialog-management/create-update-formula/create-update-formula.component';

@Component({
  selector: 'app-core-price-formula',
  templateUrl: './core-price-formula.component.html',
  styleUrls: ['./core-price-formula.component.scss']
})
export class CorePriceFormulaComponent extends BaseComponent implements OnInit {

  
  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  formula: Formula[]=[];
  
  constructor(
    protected formulaService: FormulaService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) { 
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Công thức tính giá' }
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
    this.loadFormula();
  }

  async loadFormula(): Promise<any> {
    const results = await this.formulaService.getListFormula(this.filterViewModel);
    if (results.data.length > 0) {
      this.formula = results.data;
      this.totalRecords = this.formula[0].totalCount || 0;
    } else {
      this.formula = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadFormula();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.formula = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadFormula();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadFormula();
  }


  createFormula(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdateFormulaComponent, {
      header: `${item ? 'SỬA CÔNG THỨC TÍNH GIÁ' : 'TẠO MỚI CÔNG THỨC TÍNH GIÁ'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadFormula();
      }
    });
  }

  async deleteFormula(): Promise<any> {
    const res = await this.formulaService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadFormula();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }
}
