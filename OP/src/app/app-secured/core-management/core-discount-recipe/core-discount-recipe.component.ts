import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { PromotionFormula } from 'src/app/shared/models/entity/promotionFormula.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PromotionFormulaService } from 'src/app/shared/services/api/promotionFormula.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateUpdatePromotionFormulaComponent } from '../core-dialog-management/create-update-promotion-formula/create-update-promotion-formula.component';

@Component({
  selector: 'app-core-discount-recipe',
  templateUrl: './core-discount-recipe.component.html',
  styleUrls: ['./core-discount-recipe.component.scss']
})
export class CoreDiscountRecipeComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  promotionFormula: PromotionFormula[]=[];

  constructor(
    protected promotionFormulaService: PromotionFormulaService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) { 
    super(msgService,router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý chung' },
      { label: 'Công thức giảm giá' }
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
    this.loadPromotionFormula();
  }

  async loadPromotionFormula(): Promise<any> {
    const results = await this.promotionFormulaService.getListPromotionFormula(this.filterViewModel);
    if (results.data.length > 0) {
      this.promotionFormula = results.data;
      this.totalRecords = this.promotionFormula[0].totalCount || 0;
    } else {
      this.promotionFormula = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPromotionFormula();
  }

  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.promotionFormula = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadPromotionFormula();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPromotionFormula();
  }


  createPromotionFormula(item: any = null): void {
    this.ref = this.dialogService.open(CreateUpdatePromotionFormulaComponent, {
      header: `${item ? 'SỬA CÔNG THỨC GIẢM GIÁ' : 'TẠO MỚI CÔNG THỨC GIẢM GIÁ'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadPromotionFormula();
      }
    });
  }

  async deletePromotionFormula(): Promise<any> {
    const res = await this.promotionFormulaService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPromotionFormula();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }


}
