import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { LazyLoadEvent } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { min } from 'rxjs-compat/operator/min';
import { throwIfEmpty } from 'rxjs/operators';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { SearchDate } from 'src/app/shared/infrastructure/searchDate.helper';
import { Promotion } from 'src/app/shared/models/entity/promotion.model';
import { PromotionDetail, PromotionDetailServiceDVGTs } from 'src/app/shared/models/entity/promotionDetail.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PromotionService } from 'src/app/shared/services/api/promotion.service';
import { PromotionDetailService } from 'src/app/shared/services/api/PromotionDetail.service';
import { PromotionFormulaService } from 'src/app/shared/services/api/promotionFormula.service';
import { ServiceService } from 'src/app/shared/services/api/service.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-core-deduct-price',
  templateUrl: './core-deduct-price.component.html',
  styleUrls: ['./core-deduct-price.component.scss']
})
export class CoreDeductPriceComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  promotion: Promotion = new Promotion();
  selectFromDate: any;
  selectToDate: any;
  promotionType: SelectModel[] = [];
  type: SelectModel[] = [];
  activate: SelectModel[] = [];
  selectPromotionType: SelectModel;
  selectType: SelectModel;
  selectActivate: SelectModel;
  displayConfirmCreate: boolean;
  index: number = 0;
  promotionCode: string;
  promotionName: string;
  concurrencyStamp: string;
  promotionPromotionNot: string;
  promotionTotalPromotion: number;
  promotionTotalCode: number;
  selectPromotionTypeEdit: SelectModel;
  checkIsPublic: boolean = false;
  selectIsHidden: boolean = false;
  //
  selectFromDateEditOrUpdate: any;
  selectToDateEditOrUpdate: any;
  checkUpdate: boolean = true;
  promotionFormula: SelectModel[] = [];
  service: SelectModel[] = [];
  dialogDeletePromotionDetail = false;
  selectIsHiddenPromotion: SelectModel;
  promotionDetail: PromotionDetail[] = [];
  promotionDetailData: PromotionDetail[] = [];
  totalRecordPromotionDetail: number;
  promotionFomulaFilter: Promotion = new Promotion();
  dataFakeId: number;

  constructor(
    protected serviceService: ServiceService,
    protected promotionFormulaService: PromotionFormulaService,
    protected promotionDetailService: PromotionDetailService,
    protected promotionService: PromotionService,
    protected breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý bảng giá' },
      { label: 'Danh sách giảm giá' }
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
    this.type = [
      { value: null, label: '-- Chọn dữ liệu --' },
      { value: 0, label: 'private' },
      { value: 1, label: 'public' },
    ]
    this.activate = [
      { value: null, label: '-- Chọn dữ liệu --' },
      { value: 1, label: 'hoạt động' },
      { value: 0, label: 'không hoạt động' },
    ]
    this.loadPromotion();
    this.loadPromotionType();
    this.loadPromotionFormula();
    this.laodService();
    this.dataFakeId = 0;
  }

  async loadPromotion() {
    const results = await this.promotionService.getListPromotion(this.filterViewModel);
    if (results.data.length > 0) {
      this.promotion = results.data;
      this.totalRecords = this.promotion[0].totalCount || 0;
    } else {
      this.promotion = null;
      this.totalRecords = 0;
      this.first = -1;
    }
    this.createLine();
  }

  async loadPromotionType() {
    this.promotionType = await this.promotionService.getAllPromotionTypeAsync();
  }

  async loadPromotionFormula() {
    this.promotionFormula = await this.promotionFormulaService.getListPromotionFormulaAsync()
  }

  async laodService() {
    this.service = await this.serviceService.getAllServiceAsync();
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPromotion();
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.promotion = null;
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.filterViewModel.dateFrom = null;
    this.filterViewModel.dateTo = null;
    this.selectFromDate = null;
    this.selectToDate = null;
    this.filterViewModel.isHidden = null;
    this.filterViewModel.isPublic = null;
    this.filterViewModel.promotionTypeId = null;
    this.promotionDetail = [];
    this.promotionFomulaFilter = new Promotion();
    this.checkUpdate = false;
    this.selectFromDateEditOrUpdate = null;
    this.selectToDateEditOrUpdate = null;
    this.totalRecords = 0;
    this.selectPromotionTypeEdit = null;
    this.checkIsPublic = null;
    this.selectIsHiddenPromotion = null;
    this.loadPromotion();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPromotion();
  }

  async deletePromotion() {
    const res = await this.promotionService.deleteById(this.selectedData.id);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadPromotion();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  changePromotionType() {
    this.filterViewModel.promotionTypeId = this.selectPromotionType.value;
    this.loadPromotion();
  }

  changeType() {
    this.filterViewModel.isPublic = this.selectType.value;
    this.loadPromotion();
  }

  changeActivate() {
    this.filterViewModel.isHidden = this.selectActivate.value;
    this.loadPromotion();
  }

  changeFromDate() {
    this.filterViewModel.dateFrom = SearchDate.formatToISODate(this.selectFromDate);
    if (this.selectToDate) {
      this.loadPromotion();
    }
  }

  changeToDate() {
    this.filterViewModel.dateTo = SearchDate.formatToISODate(this.selectToDate);
    this.loadPromotion();
  }

  onClickDeletePoromotion() { }

  handleChange(event: any) {
    this.index = event.index
  }

  changeIsPublic(event) {
    if (event) {
      this.promotionFomulaFilter.isPublic = event.checked;
    } else {
      this.promotionFomulaFilter.isPublic = null;
    }
  }

  async updatePromotion(data: Promotion) {
    if (data) {
      this.index = 1;
      this.checkUpdate = false;
      this.promotionFomulaFilter.code = data.code
      this.promotionFomulaFilter.promotionNot = data.promotionNot;
      this.promotionFomulaFilter.totalPromotion = data.totalPromotion;
      this.promotionFomulaFilter.totalCode = data.totalCode;
      this.promotionFomulaFilter.isHidden = data.isHidden;
      this.promotionFomulaFilter.isPublic = data.isPublic;
      this.promotionFomulaFilter.promotionTypeId = data.promotionTypeId;
      this.promotionFomulaFilter.fromDate = data.fromDate;
      this.promotionFomulaFilter.toDate = data.toDate;
      this.selectIsHiddenPromotion = data.isHidden;
      this.checkIsPublic = data.isPublic;
      const findPromotionTypeEdit = this.promotionType.find(f => f.value === data.promotionTypeId);
      if (findPromotionTypeEdit) {
        this.selectPromotionTypeEdit = findPromotionTypeEdit;
      }
      if (data.fromDate) {
        this.selectFromDateEditOrUpdate = new Date(data.fromDate);
      }
      if (data.toDate) {
        this.selectToDateEditOrUpdate = new Date(data.toDate);
      }
      this.promotionFomulaFilter.id = data.id;
      await this.getListPromotionDetail();
    }
  }

  editPromotionDetail() {
    this.refresher();
    this.checkUpdate = true;
  }

  async getListPromotionDetail() {
    if (this.selectFromDateEditOrUpdate) {
      this.promotion.fromDate = SearchDate.formatToISODate(this.selectFromDateEditOrUpdate);
    }
    if (this.selectToDateEditOrUpdate) {
      this.promotion.toDate = SearchDate.formatToISODate(this.selectToDateEditOrUpdate);
    }
    this.promotionDetailData = await this.promotionDetailService.getListPromotionDetailByPromotionId(this.promotionFomulaFilter.id);
    if (this.promotionDetailData.length > 0) {
      this.totalRecordPromotionDetail = this.promotionDetailData.length;
      this.promotionDetail = [];
      for (const key in this.promotionDetailData) {
        const el = this.promotionDetailData[key];
        const resultPromotionDetail = await this.mapingPromotionDetail(el, key);
        this.promotionDetail.push(resultPromotionDetail);
      }
    }
  }


  async mapingPromotionDetail(el: PromotionDetail, index: any) {
    const promotionDetail = new PromotionDetail();
    promotionDetail.fakeId = Number(index);
    promotionDetail.valueFrom = el.valueFrom;
    promotionDetail.valueTo = el.valueTo;
    promotionDetail.value = el.value;
    const findpromotionFormula = this.promotionFormula.find(f => f.value === el.promotionFormulaId);
    if (findpromotionFormula) {
      promotionDetail.promotionFormulaName = findpromotionFormula.label;
      promotionDetail.promotionFormulaId = findpromotionFormula.value;
    }
    let promotionDetailServiceDVGTs = [];
    el.serviceDVGTs = [];
    el.dataServiceDVGTs = [];
    if (el.promotionDetailServiceDVGTs.length > 0) {
      for (const keys in el.promotionDetailServiceDVGTs) {
        const es = el.promotionDetailServiceDVGTs[keys];
        const resultPromotionDetailServiceDVGTs = await this.mapingPromotionDetailServiceDVGTs(es, keys);
        const findService = this.service.find(f => f.value === es.serviceId);
        if (findService) {
          el.dataServiceDVGTs.push(findService);
        }
        el.serviceDVGTs.push(resultPromotionDetailServiceDVGTs.id);
        promotionDetailServiceDVGTs.push(resultPromotionDetailServiceDVGTs);
      }
      el.selectedItemsLabel = `Đã chọn ${promotionDetailServiceDVGTs.length} chức vụ`
    }
    promotionDetail.serviceDVGTs = el.serviceDVGTs;
    promotionDetail.selectedItemsLabel = el.selectedItemsLabel;
    promotionDetail.promotionDetailServiceDVGTs = promotionDetailServiceDVGTs;
    promotionDetail.dataServiceDVGTs = el.dataServiceDVGTs;
    return promotionDetail;
  }

  mapingPromotionDetailServiceDVGTs(es: PromotionDetailServiceDVGTs, keys: any) {
    const promotionDetailServiceDVGTs = new PromotionDetailServiceDVGTs();
    promotionDetailServiceDVGTs.id = 0;
    promotionDetailServiceDVGTs.serviceId = es.serviceId;
    promotionDetailServiceDVGTs.promotionDetailId = es.promotionDetailId;
    promotionDetailServiceDVGTs.isEnabled = es.isEnabled;
    return promotionDetailServiceDVGTs;
  }

  createLine() {
    let newPromotionDetail = new PromotionDetail();
    let leng = this.promotionDetail.length;
    newPromotionDetail = this.clone(this.promotionDetail[(leng - 1)]);
    newPromotionDetail.id = null;
    newPromotionDetail.promotionFormulaId = null;
    newPromotionDetail.promotionFormulaName = null;
    newPromotionDetail.dataServiceDVGTs = null;
    newPromotionDetail.selectedItemsLabel = null;
    newPromotionDetail.valueFrom = 0;
    newPromotionDetail.valueTo = 0;
    newPromotionDetail.value = 0;
    newPromotionDetail.serviceDVGTs = null;
    newPromotionDetail.promotionId = null;
    newPromotionDetail.promotionDetailServiceDVGTs = []
    newPromotionDetail.concurrencyStamp = null;
    newPromotionDetail.promotionFormulaId = null;
    newPromotionDetail.fakeId++;
    this.promotionDetail.push(newPromotionDetail);
    this.totalRecordPromotionDetail++;
  }

  onChangePromotionFormula(data: PromotionDetail) {
    if (this.checkUpdate) {
      data.fakeId = this.dataFakeId++;
      let findPromotionDetail = this.promotionDetail.find(f => f.fakeId === data.fakeId);
      if (findPromotionDetail) {
        const findpromotionFormula = this.promotionFormula.find(f => f.value === data.promotionFormulaId);
        if (findpromotionFormula) {
          findPromotionDetail.promotionFormulaId = findpromotionFormula.value;
          findPromotionDetail.promotionFormulaName = findpromotionFormula.label;
        }
      }
    } else { 
      let findPromotionDetail = this.promotionDetail.find(f => f.fakeId === data.fakeId);
      if (findPromotionDetail) {
        const findpromotionFormula = this.promotionFormula.find(f => f.value === data.promotionFormulaId);
        if (findpromotionFormula) {
          findPromotionDetail.promotionFormulaId = findpromotionFormula.value;
          findPromotionDetail.promotionFormulaName = findpromotionFormula.label;
        }
      }
    }
  }

  onChangeService(data: PromotionDetail) {
    let findPromotionDetail = this.promotionDetail.find(f => f.fakeId === data.fakeId);
    if (findPromotionDetail) {
      findPromotionDetail.promotionDetailServiceDVGTs = [];
      findPromotionDetail.serviceDVGTs = [];
      findPromotionDetail.dataServiceDVGTs.map(x => {
        findPromotionDetail.promotionDetailServiceDVGTs.push({ id: 0, isEnabled: true, promotionDetailId: 0, serviceId: x.value });
        findPromotionDetail.serviceDVGTs.push(x.value);
      });
      findPromotionDetail.selectedItemsLabel = `Đã chọn ${data.dataServiceDVGTs.length} chức vụ`;
    }
  }

  changeActivatePromotion(event) {
    if (event) {
      this.promotionFomulaFilter.isHidden = event.checked;
    } else {
      this.promotionFomulaFilter.isHidden = null;
    }
  }

  changePromotionTypeEdit() {
    this.promotionFomulaFilter.promotionTypeId = this.selectPromotionTypeEdit.value;
  }

  async createOrUpdatePromotionDetail() {
    if (this.promotionFomulaFilter.totalPromotion == 0) {
      this.promotionFomulaFilter.totalPromotion = null;
    }
    if (this.promotionFomulaFilter.totalCode == 0) {
      this.promotionFomulaFilter.totalCode = null;
    }
    if (this.selectFromDateEditOrUpdate) {
      this.promotionFomulaFilter.fromDate = SearchDate.formatToISODate(this.selectFromDateEditOrUpdate);
    }
    else {
      this.promotionFomulaFilter.fromDate = null;
    }
    if (this.selectToDateEditOrUpdate) {
      this.promotionFomulaFilter.toDate = SearchDate.formatToISODate(this.selectToDateEditOrUpdate);
    }
    else {
      this.promotionFomulaFilter.toDate = null;
    }
    this.promotionFomulaFilter.promotionDetails = this.promotionDetail;
    if (this.checkUpdate) {
      if (!this.createIsValidPromotion()) return;
      const res = await this.promotionService.create(this.promotionFomulaFilter);
      if (res.isSuccess) {
        this.msgService.success('Tạo mới thành công!');
        this.refresher();
      }
      else {
        this.messageService.error(res.message);
      }
    } else {
      if (!this.updateIsValidPromotion()) return;
      const res = await this.promotionService.update(this.promotionFomulaFilter);
      if (res.isSuccess) {
        this.msgService.success('Cập nhật thành công!');
        this.refresher();
      }
      else {
        this.messageService.error(res.message);
      }
    }
  }

  createIsValidPromotion(): boolean {

    if (!this.promotionFomulaFilter.code) {
      this.msgService.error('Vui lòng mã giảm giá!');
      return false;
    }

    if (!this.selectFromDateEditOrUpdate) {
      this.msgService.error('Vui lòng nhập ngày bắt đầu!');
      return false;
    }

    if (!this.selectToDateEditOrUpdate) {
      this.msgService.error('Vui lòng nhập ngày kết thúc!');
      return false;
    }

    if (!this.promotionTotalPromotion && this.promotionTotalPromotion < 0) {
      this.msgService.error('Vui lòng nhập tổng ngân sách là số dương!');
      return false;
    }

    if (!this.promotionTotalCode && this.promotionTotalCode < 0) {
      this.msgService.error('Vui lòng nhập tổng số mã phát hành là số dương!');
      return false;
    }

    if (this.selectFromDateEditOrUpdate && (SearchDate.formatToISODate(this.selectFromDateEditOrUpdate) < SearchDate.formatToISODate(new Date()))) {
      this.msgService.error('Từ ngày phải lớn hơn hoặc bằng ngày hiện tại!');
      return false;
    }

    if (this.selectToDateEditOrUpdate && (SearchDate.formatToISODate(this.selectToDateEditOrUpdate) < SearchDate.formatToISODate(new Date()))) {
      this.msgService.error('Đến ngày phải lớn hơn hoặc bằng ngày hiện tại!');
      return false;
    }

    if (this.selectFromDateEditOrUpdate && this.selectToDateEditOrUpdate && this.selectFromDateEditOrUpdate > this.selectToDateEditOrUpdate) {
      this.msgService.error('Đến ngày phải lớn hơn từ ngày!');
      return false;
    }

    if (!this.selectPromotionTypeEdit) {
      this.msgService.error('Vui lòng chọn tính theo!');
      return false;
    }

    return true;
  }


  updateIsValidPromotion(): boolean {

    if (!this.promotionFomulaFilter.code) {
      this.msgService.error('Vui lòng mã giảm giá!');
      return false;
    }

    if (!this.selectFromDateEditOrUpdate) {
      this.msgService.error('Vui lòng nhập ngày bắt đầu!');
      return false;
    }

    if (!this.selectToDateEditOrUpdate) {
      this.msgService.error('Vui lòng nhập ngày kết thúc!');
      return false;
    }

    if (!this.promotionTotalPromotion && this.promotionTotalPromotion < 0) {
      this.msgService.error('Vui lòng nhập tổng ngân sách là số dương!');
      return false;
    }

    if (!this.promotionTotalCode && this.promotionTotalCode < 0) {
      this.msgService.error('Vui lòng nhập tổng số mã phát hành là số dương!');
      return false;
    }

    if (this.selectFromDateEditOrUpdate && this.selectToDateEditOrUpdate && this.selectFromDateEditOrUpdate > this.selectToDateEditOrUpdate) {
      this.msgService.error('Đến ngày phải lớn hơn từ ngày!');
      return false;
    }

    if (!this.selectPromotionTypeEdit) {
      this.msgService.error('Vui lòng chọn tính theo!');
      return false;
    }


    return true;
  }

  async deletePromotionDetail(): Promise<any> {
    this.promotionDetail.splice(this.index, 1);
    this.totalRecordPromotionDetail--;
    this.dialogDeletePromotionDetail = false;
    this.messageService.success('xoá dòng thành công!');
  }
}
