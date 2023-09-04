import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { SearchDate } from 'src/app/shared/infrastructure/searchDate.helper';
import { AreaGroup } from 'src/app/shared/models/entity/areaGroup.model';
import { Area } from 'src/app/shared/models/entity/area.model';
import { PriceList } from 'src/app/shared/models/entity/priceList.model';
import { PriceService } from 'src/app/shared/models/entity/priceService.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { WeightGroup } from 'src/app/shared/models/entity/weightGroup.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AreaGroupService } from 'src/app/shared/services/api/areaGroup.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PriceServiceService } from 'src/app/shared/services/api/priceService.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { ServiceService } from 'src/app/shared/services/api/service.service';
import { WeightGroupService } from 'src/app/shared/services/api/weightGroup.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';
import { Weight } from 'src/app/shared/models/entity/weight.model';
import { AreaAndSelect } from 'src/app/shared/models/entity/areaAndSelected.model';
import { DataPriceServiceDetails } from 'src/app/shared/models/entity/dataPriceServiceDetails.model';
import { PriceServiceDetail } from 'src/app/shared/models/entity/priceServiceDetail.model';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { CustomerPriceServiceService } from 'src/app/shared/services/api/customer-price-service.service';
import { PriceServiceDetailService } from 'src/app/shared/services/api/priceServiceDetail.service';
import { SortUtil } from 'src/app/shared/infrastructure/sort.util';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { WeightService } from 'src/app/shared/services/api/weight.service';
import { AreaAndPrice } from 'src/app/shared/models/entity/areaAndPrice.model';
import { CustomerPriceServiceModel } from 'src/app/shared/models/entity/CustomerPriceServiceModel.model';
import { DataFilterViewModel } from 'src/app/shared/models/entity/dataFilter.viewModel';
import { AreaService } from 'src/app/shared/services/api/area.service';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { CoreCopyPriceComponent } from '../core-dialog-price-management/core-copy-price/core-copy-price.component';
import { PricingTypeService } from 'src/app/shared/services/api/pricingType.service';
import { FormulaService } from 'src/app/shared/services/api/formula.service';

@Component({
  selector: 'app-core-price-list',
  templateUrl: './core-price-list.component.html',
  styleUrls: ['./core-price-list.component.scss']
})
export class CorePriceListComponent extends BaseComponent implements OnInit {

  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  onPageChangeEvent: any;
  // danh sách bảng gía
  selectFromDate: any;
  listPriceServiceBeta: PriceService[] = [];
  selectToDate: any;
  selectedFromProvinceFrom: SelectModel;
  selectedToProvince: SelectModel;
  provinces: SelectModel[] = [];
  services: SelectModel[] = [];
  searchText: string;
  selectedServiceFilter: SelectModel;
  // tạo bảng giá
  priceService: PriceService = new PriceService();
  resultPriceServices: string[] = [];
  listPriceService: PriceService[] = [];
  selectedService: number;
  selecNum: SelectModel[] = [
    { label: "0", value: 0 },
    { label: "1", value: 1 },
    { label: "2", value: 2 },
    { label: "3", value: 3 },
    { label: "4", value: 4 },
    { label: "5", value: 5 },
    { label: "6", value: 6 },
    { label: "7", value: 7 },
    { label: "8", value: 8 },
    { label: "9", value: 9 }
  ];
  selectedPriceService: number;
  selectedPriceList: number;
  priceLists: SelectModel[] = [];
  selectCreateDateFrom: string;
  selectCreateDateTo: string;
  newAreas: Area[] = [];
  newWeights: Weight[] = [];
  isAddPS: boolean = true;
  customerPriceServiceModel: CustomerPriceServiceModel = new CustomerPriceServiceModel();
  areaAndSelect: AreaAndSelect[] = [];
  dataPriceListDetail: DataPriceServiceDetails[] = [];
  weights: Weight[] = [];
  priceServiceDetails: PriceServiceDetail[] = [];
  priceServices: SelectModel[] = [];
  selectedCustomer: CustomerPriceServiceModel[] = [];
  selct_arr: any[] = [];
  countAreas: number = 0;
  clonePriceServiceDetail: any;
  areas: Area[] = [];
  addwieght_pat: number[] = [];
  lstDistricts: SelectModel[] = [];
  selectedPriceServiceDetail: PriceServiceDetail;
  areaAndPrice: AreaAndPrice[] = [];
  lstCloneAreaSelect: AreaAndSelect[] = [];
  selectedWeight: any;
  cloneWeightTo: any;
  valuetoNum: any;
  valuefromNum: any;
  index: any = 0;
  selectedArea: Area;
  serviceId: number;
  numOrderId: number;
  selectedServices: SelectModel;
  selectedNumOrder: SelectModel;
  pricingType: SelectModel[] = [];
  formulas: SelectModel[] = [];
  isEditPrice: boolean = true;
  selectedPricingType: SelectModel;
  selectedFormula: SelectModel;

  constructor(
    protected permissionService: PermissionService,
    private breadcrumbService: BreadcrumbService,
    protected dialogService: DialogService,
    private msgService: MsgService,
    protected router: Router,
    // danh sách bảng gía
    private priceServiceService: PriceServiceService,
    private provinceService: ProvinceService,
    private serviceService: ServiceService,
    // tạo bảng giá
    private areaGroupService: AreaGroupService,
    private weightGroupService: WeightGroupService,
    private customerPriceService: CustomerPriceServiceService,
    private priceServiceDetailService: PriceServiceDetailService,
    private districtService: DistrictService,
    private weightService: WeightService,
    private areaService: AreaService,
    protected pricingTypeService: PricingTypeService,
    protected formulaService: FormulaService,

  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý bảng giá' },
      { label: 'Thông tin bảng giá' }
    ]);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData(): Promise<void> {
    this.filterViewModel = {
      pageNumber: 1,
      pageSize: 20,
    };
    this.loadPricingType();
    this.loadFormulas();
    this.loadProvince();
    this.loadService();
    this.loadListDistrict();
    this.loadPriceServices();
  }
  // Danh sách bảng giá
  async loadProvince(): Promise<any> {
    this.provinces = await this.provinceService.getProvinceAsync();
  }

  async loadService(): Promise<any> {
    this.services = await this.serviceService.getAllServiceAsync();
  }

  async loadPricingType(): Promise<any> {
    this.pricingType = await this.pricingTypeService.getAllPricingTypeAsync();
  }

  async loadFormulas(): Promise<any> {
    this.formulas = await this.formulaService.getAllMultiSelectModelAsync();
  }

  async loadPriceServices(): Promise<any> {
    const results = await this.priceServiceService.getListPriceService(this.filterViewModel);
    if (results.data.length > 0) {
      this.listPriceServiceBeta = results.data;
      this.totalRecords = this.listPriceServiceBeta[0].totalCount || 0;
    } else {
      this.listPriceServiceBeta = [];
      this.totalRecords = 0;
      this.first = -1;
    }
  }
  handleChange(event: any): void {
    this.index = event.index;
  }

  // chọn từ ngày
  changeFromDate(): void {
    if (this.selectFromDate && this.selectToDate && this.selectFromDate > this.selectToDate) {
      this.msgService.error('Đến ngày phải lớn hơn từ ngày!');
      return;
    }
    this.filterViewModel.dateFrom = SearchDate.formatToISODate(this.selectFromDate);
    if (this.selectToDate) {
      this.loadPriceServices();
    }
  }

  changeToDate(): void {
    if (this.selectFromDate && this.selectToDate && this.selectFromDate > this.selectToDate) {
      this.msgService.error('Đến ngày phải lớn hơn từ ngày!');
      return;
    }
    this.filterViewModel.dateTo = SearchDate.formatToISODate(this.selectToDate);
    this.loadPriceServices();
  }

  changeService(): void {
    this.filterViewModel.serviceId = this.selectedServiceFilter.value;
    this.loadPriceServices();
  }

  changeFromProvince(): void {
    this.filterViewModel.provinceFromId = this.selectedFromProvinceFrom.value;
    this.loadPriceServices();
  }

  changeToProvince(): void {
    this.filterViewModel.provinceToId = this.selectedToProvince.value;
    this.loadPriceServices();
  }

  onFilter(): void {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadPriceServices();
  }

  onSelectedByCode(event) {
    let value = event;
    this.findByCode(value);
    this.onViewPriceListDetail();
  }


  refresher(): void {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.filterViewModel.provinceFromId = null;
    this.filterViewModel.provinceToId = null;
    this.filterViewModel.serviceId = null;
    this.filterViewModel.dateFrom = null;
    this.filterViewModel.dateTo = null;
    this.selectFromDate = null;
    this.selectToDate = null;
    this.selectedFromProvinceFrom = null;
    this.selectedToProvince = null;
    this.selectedService = null;
    this.listPriceServiceBeta = [];
    this.totalRecords = 0;
    this.first = 0;
    this.searchText = '';
    this.loadPriceServices();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadPriceServices();
  }
  // tạo bảng giá
  refresCreateprice() {

    this.loadPriceServices();
  }

  searchByCode(event) {
    let value = event.query;
    if (value.length >= 1) {
      this.priceServiceService.getByCodeAsync(value).then(
        x => {
          this.listPriceService = x as PriceService[];
          this.resultPriceServices = this.listPriceService.map(f => f.code);
        }
      );
    }
  }

  keyUpEnter(event) {
    if (event.code == 'Enter') {
      let value = event.currentTarget.value;
      this.findFilterByCode(value);
      this.onViewPriceListDetail();
    }
  }

  async loadListDistrict() {
    this.lstDistricts = await this.districtService.getAllSelectModelAsync();
  }

  onChangeService() {
    this.serviceId = this.selectedServices.value;
  }

  onChangeNumOrder(event) {
    let eventNumOrderId = event.value
    this.numOrderId = eventNumOrderId.value;
  }

  async AddNewPriceService() {
    if (!this.isValidResponseCreatePrice()) return;

    let priceList = new PriceList();
    if (this.selectedPriceList) {
      priceList = this.priceLists.find(f => f.value == this.selectedPriceList).data;
    }
    let areaGroup = new AreaGroup();
    if (priceList.id) {
      areaGroup.hubId = priceList.hubId;
      areaGroup.code = priceList.code + this.selectedService;
      areaGroup.name = areaGroup.code;
      areaGroup.isAuto = true;
      await this.areaGroupService.createAsync(areaGroup).then(
        x => {
          if (!this.isValidResponse(x)) return;
          areaGroup = x.data;
        }
      )
    }
    let weightGroup = new WeightGroup();
    if (priceList.id) {
      weightGroup.code = priceList.id + '-' + this.selectedService;
      weightGroup.name = weightGroup.code;
      weightGroup.isAuto = true;
      await this.weightGroupService.createAsync(weightGroup).then(
        x => {
          if (!this.isValidResponse(x)) return;
          weightGroup = x.data;
        }
      );
    }
    let priceService = new PriceService();
    priceService.priceListId = this.selectedPriceList || null;
    priceService.serviceId = this.serviceId;
    priceService.areaGroupId = areaGroup.id || null;
    priceService.weightGroupId = weightGroup.id || null;
    priceService.code = this.priceService.code;
    priceService.name = this.priceService.code;
    priceService.isAuto = true;
    //
    priceService.numOrder = this.numOrderId ? this.numOrderId : 0;
    priceService.isTwoWay = this.priceService.isTwoWay;
    priceService.isPublic = this.priceService.isPublic;
    priceService.isKeepWeight = this.priceService.isKeepWeight;
    priceService.publicDateFrom = this.selectCreateDateFrom ? moment(this.selectCreateDateFrom).format(environment.formatDate) : null;
    priceService.publicDateTo = this.selectCreateDateTo ? moment(this.selectCreateDateTo).format(environment.formatDate) : null;
    //
    priceService.pricingTypeId = this.priceService.pricingTypeId != null ? this.priceService.pricingTypeId : 1;
    priceService.vatPercent = this.priceService.vatPercent || 0;
    priceService.vatSurcharge = this.priceService.vatSurcharge || 0;
    priceService.fuelPercent = this.priceService.fuelPercent || 0;
    priceService.dim = this.priceService.dim || 0;
    priceService.structureId = this.priceService.structureId || null;

    await this.priceServiceService.createAsync(priceService).then(
      x => {
        if (!this.isValidResponse(x)) return;
        this.listPriceService.push(x.data as PriceService);
        this.newAreas = [];
        this.newWeights = [];
        this.isAddPS = true;
        this.findByCode(this.priceService.code);
        this.msgService.success("Tạo bảng giá dịch vụ thành công");
        this.onViewPriceListDetail();
        this.loadPriceServices();
      }
    );
  }

  isValidResponseCreatePrice(): boolean {
    if (this.selectedPriceService) {
      this.msgService.error("Bảng giá đã tồn tại, vui lòng kiểm tra lại");
      return false;
    }

    if (!this.selectedService) {
      this.msgService.error("Vui lòng chọn dịch vụ");
      return false;
    }

    if (!this.priceService.code) {
      this.msgService.error("Vui lòng nhập mã bảng giá dịch vụ");
      return false;
    }

    return true
  }

  async findByCode(code: string) {
    this.areaAndSelect = [];
    this.dataPriceListDetail = [];
    this.weights = []
    this.selectCreateDateFrom = null;
    this.selectCreateDateTo = null;
    this.priceServices = [];
    this.priceServiceDetails = [];
    if (code.length >= 1) {
      let findPS = this.listPriceService.find(f => f.code.toLocaleUpperCase() == code.toLocaleUpperCase());
      if (findPS) {
        this.priceService = findPS;
        this.priceServices.push({ label: findPS.code + ' - ' + findPS.name, value: findPS.id, data: findPS });
        this.selectedPriceService = findPS.id;
        this.selectedService = findPS.serviceId;
        const finServices = this.services.find(x => x.value === findPS.serviceId);
        if (finServices) {
          this.selectedServices = finServices;
        }
        const finNumOrder = this.selecNum.find(x => x.value === findPS.numOrder);
        if (finNumOrder) {
          this.selectedNumOrder = finNumOrder;
        }
        this.selectedPriceList = findPS.priceListId;
        this.selectCreateDateFrom = findPS.publicDateFrom ? moment(findPS.publicDateFrom).format(environment.formatDate) : null;
        this.selectCreateDateTo = findPS.publicDateTo ? moment(findPS.publicDateTo).format(environment.formatDate) : null;
        this.getCustomerPriceService();
      } else {
        this.msgService.error("Bảng giá dịch vụ mới");
        this.priceService = new PriceService();
        this.priceService.fuelPercent = 0;
        this.priceService.vatPercent = 0;
        this.priceService.vatSurcharge = 0;
        this.priceService.dim = 0;
        this.priceService.numOrder = 0;
        this.priceService.pricingTypeId = 1;
        this.priceService.code = code;
        this.priceService.numOrder = 1;
        this.priceServices = [];
        this.selectedPriceService = null;
        this.selectedService = null;
        this.selectedPriceList = null;
        this.selectCreateDateFrom = null;
        this.selectCreateDateTo = null;
        this.priceServiceDetails = [];
        this.selectedCustomer = [];
      }
    }
  }

  async onViewPriceListDetail() {
    this.messageService.clear();
    if (!this.selectedPriceService) {
      this.msgService.error("Vui lòng chọn mã bảng giá cần xem");
      return;
    }
    this.priceService = new PriceService();
    this.areaAndSelect = [];
    this.selct_arr = [];
    this.dataPriceListDetail = [];
    this.weights = [];
    this.countAreas = 0;
    // this.dateFrom = null;
    // this.dateTo = null;
    this.priceServiceDetails = [];
    this.priceService = this.priceServices.find(f => f.value == this.selectedPriceService).data;
    if (this.priceService) {
      let priceService = this.priceServices.find(f => f.value == this.selectedPriceService).data;
      if (priceService) {
        this.customerPriceServiceModel = priceService;
      }
      const data = await this.priceServiceDetailService.getByPriceServiceIdAsync(priceService ? priceService.areaGroupId : null, this.selectedPriceService);
      this.clonePriceServiceDetail = data;
      if (this.newAreas) {
        await Promise.all(this.newAreas.map(x => {
          if (!data.areas) data.areas = [];
          if (!data.areas.find(f => f.id == x.id))
            data.areas.push(x);
        }));

      }

      if (data.areas.length > 0) {
        this.areaAndSelect = this.areas = data.areas;
        this.countAreas = data.areas.length + 1;
        for (var i = 0; i < this.areaAndSelect.length; i++) this.addwieght_pat[i] = 0;
        this.priceServiceSelectChange(data);
      }
    }
    else {

      this.msgService.error("Không có dữ liệu");
    }
    this.selct_arr = this.dataPriceListDetail;
  }

  async getCustomerPriceService() {
    if (this.selectedPriceService) {
      let result = await this.customerPriceService.GetByPriceServiceId(this.selectedPriceService);
      this.selectedCustomer = result;
      this.totalRecords = this.selectedCustomer.length;
    }
    else {
      this.selectedCustomer = [];
    }
  }

  async priceServiceSelectChange(data) {
    this.areaAndSelect.map(async x => {
      x.multiSelectProvinces = [];
      x.multiSelectDistrict = [];
      x.multiSelectFromProvince = [];
      x.multiSelectFromDistrict = [];
      x.districts = [];
      x.filterFromProvince = `Chọn T/T đi`;
      x.filterFromDistrict = `Chọn Q/H đi`;
      x.filterToProvince = `Chọn T/T đến`;
      x.filterToDistrict = `Chọn Q/H đến`;

      await Promise.all(data.districtSelected.map(dist => {
        if (dist.areaId == x.id) {
          if (x.multiSelectProvinces.indexOf(dist.provinceId) == -1) x.multiSelectProvinces.push(dist.provinceId);
          if (x.multiSelectDistrict.indexOf(dist.districtId) == -1) x.multiSelectDistrict.push(dist.districtId);
        }
      }));
      await Promise.all(data.fromProvinces.map(dist => {
        if (dist.areaId == x.id) {
          if (x.multiSelectFromProvince.indexOf(dist.provinceId) == -1) x.multiSelectFromProvince.push(dist.provinceId);
        }
      }));
      await Promise.all(data.fromDistricts.map(dist => {
        if (dist.areaId == x.id) {
          if (x.multiSelectFromDistrict.indexOf(dist.districtId) == -1) x.multiSelectFromDistrict.push(dist.districtId);
        }
      }));
      //
      await Promise.all(this.lstDistricts.map(dist => {
        if (dist.value) {
          if (x.multiSelectProvinces.find(pro => pro == dist.data.provinceId))
            x.districts.push(dist);
          if (x.multiSelectFromProvince.find(pro => pro == dist.data.provinceId))
            x.fromDistricts.push(dist);
        }
      }));
      x.districts = SortUtil.sortAlphanumerical(x.districts, 1, 'label');
      //x.fromDistricts = SortUtil.sortAlphanumerical(x.fromDistricts, 1, 'label');
      //
      let fromProvinceSelected = data.fromProvinces.filter(fpro => fpro.areaId == x.id);
      x.multiSelectFromProvince = fromProvinceSelected.map(x => x.provinceId);
    });
    // if (this.structureLabel) {
    //   // console.log(this.structures.find(f => f.label === this.structureLabel).value);
    // } else {
    //   // this.priceService.structureId = null
    // }
    let weightGroupData = this.priceServices.find(f => f.value == this.selectedPriceService).data;
    let weightGroupId = weightGroupData ? weightGroupData.weightGroupId : null;
    const weights = await this.weightService.getByWeightGroupAsync(weightGroupId, this.selectedPriceService);

    if (weights) {
      const priceServiceDetail = new PriceServiceDetail();
      const weight: Weight = new Weight();
      priceServiceDetail.weight = weights.data
      this.selectedWeight = [];
      //for weight
      for (const keys in priceServiceDetail.weight) {
        const es = priceServiceDetail.weight[keys];
        const selectedWeight = await this.mapSelectedWeight(es, keys);
        if (selectedWeight) {
          this.selectedWeight.push(selectedWeight);
        } else {
          this.selectedWeight = [];
        }
      }
      this.weights = weights.data as Weight[];
      this.isEditPrice = false;
      console.log(this.selectedWeight);
    }
    this.newWeights.map(x => {
      if (!this.weights.find(f => f.id == x.id))
        this.weights.push(x);
    });
    //
    const modelPriceServiceDetail = new Object();
    const weightIds = [];
    this.weights.map(m => {
      weightIds.push(m.id);
    })
    modelPriceServiceDetail['weightIds'] = weightIds;
    modelPriceServiceDetail['priceServiceId'] = this.selectedPriceService;
    const dataPriceServiceDetail = await this.priceServiceDetailService.getByPriceServiceDetailAsync(modelPriceServiceDetail);
    this.priceServiceDetails = dataPriceServiceDetail;
    if (this.priceServiceDetails && this.priceServiceDetails.length && this.priceServiceDetails[0].priceService) {
      this.selectedPriceServiceDetail = this.priceServiceDetails[0];
    }
    // get areaAndPrice
    this.areaAndPrice = [];

    let result: DataPriceServiceDetails[] = [];
    for (let i = 0; i < this.weights.length; i++) {
      let obj: DataPriceServiceDetails = new DataPriceServiceDetails;
      let row = this.weights[i] as Weight;
      // lấy ra row theo từng Weight
      obj.weight = row;
      let areaAndPrices = [];
      // lấy ra column area và giá
      await Promise.all(this.areaAndSelect.map(x => {
        if (this.priceServiceDetails) {
          let priceDetail = this.priceServiceDetails.find(f => f.areaId == x.id && f.weightId == row.id);
          if (priceDetail) {
            let areaAndPrice: AreaAndPrice = new AreaAndPrice;
            areaAndPrice.area = x;
            areaAndPrice.priceBasic = priceDetail;
            areaAndPrices.push(areaAndPrice);
          } else {
            let areaAndPrice: AreaAndPrice = new AreaAndPrice;
            areaAndPrice.area = x;
            let priceDetail = new PriceServiceDetail();
            priceDetail.areaId = x.id;
            priceDetail.area = x;
            priceDetail.weight = row;
            priceDetail.weightId = row.id;
            priceDetail.priceServiceId = this.selectedPriceService;
            priceDetail.price = 0;
            priceDetail.id = 0;
            areaAndPrice.priceBasic = priceDetail;
            this.priceServiceDetails.push(areaAndPrice.priceBasic);
            areaAndPrices.push(areaAndPrice);
          }
        }
      }));
      obj.areaAndPrices = areaAndPrices;
      result.push(obj);
    }
    if (result.length > 0) {
      this.dataPriceListDetail = result;
      this.selct_arr = this.dataPriceListDetail;
    }
    this.lstCloneAreaSelect = [];
    this.lstCloneAreaSelect = JSON.parse(JSON.stringify(this.areaAndSelect));
  }

  mapSelectedWeight(item: Weight, index: any) {
    const selectedWeight = new Weight();
    selectedWeight.fakeId = Number(index);
    selectedWeight.formula = item.formula;
    selectedWeight.formulaId = item.formulaId;
    selectedWeight.isAuto = item.isAuto;
    selectedWeight.isEnabled = item.isEnabled;
    selectedWeight.isWeightCal = item.isWeightCal;
    selectedWeight.structure = item.structure;
    selectedWeight.structureId = item.structureId;
    selectedWeight.valueFrom = item.valueFrom;
    selectedWeight.valueTo = item.valueTo;
    selectedWeight.weightFrom = item.weightFrom;
    selectedWeight.weightGroup = item.weightGroup;
    selectedWeight.weightGroupId = item.weightGroupId;
    selectedWeight.weightPlus = item.weightPlus;
    selectedWeight.weightTo = item.weightTo;
    const findPricingType = this.pricingType.find(x => x.value === item.pricingTypeId);
    if (findPricingType) {
      this.selectedPricingType = findPricingType;
    }

    const findFormula = this.formulas.find(x => x.value === item.formulaId);
    if (findFormula) {
      this.selectedFormula = findFormula;
    }

    return selectedWeight;
  }

  findFilterByCode(code: string) {
    this.messageService.clear();
    this.areaAndSelect = [];
    this.dataPriceListDetail = [];
    this.weights = []
    this.selectCreateDateFrom = null;
    this.selectCreateDateTo = null;
    this.priceServiceDetails = [];
    if (code.length >= 1) {
      let findPS = this.listPriceService.find(f => f.code.toLocaleUpperCase().indexOf(code.toLocaleUpperCase()) > -1);
      if (findPS) {
        this.priceService = findPS;
        this.priceServices = this.listPriceService.map(f => { return { label: f.code + ' - ' + f.name, value: f.id, data: f } });
        this.selectedPriceService = findPS.id;
        this.selectedService = findPS.serviceId;
        this.selectedPriceList = findPS.priceListId;
        this.selectCreateDateFrom = findPS.publicDateFrom ? moment(findPS.publicDateFrom).format(environment.formatDate) : null;
        this.selectCreateDateTo = findPS.publicDateTo ? moment(findPS.publicDateTo).format(environment.formatDate) : null;
        this.getCustomerPriceService();
        this.isAddPS = true;
      } else {
        this.priceServiceService.getByCodeAsync(code).then(
          x => {
            this.listPriceService = x as PriceService[];
            let findPSR = this.listPriceService.find(f => f.code.toLocaleUpperCase().indexOf(code.toLocaleUpperCase()) > -1);
            if (findPSR) {
              this.priceService = findPSR;
              this.priceServices = this.listPriceService.map(f => { return { label: f.code + ' - ' + f.name, value: f.id, data: f } });
              this.selectedPriceService = findPSR.id;
              this.selectedService = findPSR.serviceId;
              this.selectedPriceList = findPSR.priceListId;
              this.selectCreateDateFrom = findPSR.publicDateFrom ? moment(findPSR.publicDateFrom).format(environment.formatDate) : null;
              this.selectCreateDateTo = findPSR.publicDateTo ? moment(findPSR.publicDateTo).format(environment.formatDate) : null;
              this.getCustomerPriceService();
              this.isAddPS = true;
            } else {
              this.isAddPS = false;
              this.messageService.add({ severity: Constant.messageStatus.warn, detail: "Bảng giá mới" });
              this.priceService = new PriceService();
              this.priceService.code = code;
              this.priceServices = [];
              this.selectedPriceService = null;
              this.selectedService = null;
              this.selectedPriceList = null;
              this.selectCreateDateFrom = null;
              this.selectCreateDateTo = null;
              this.priceServiceDetails = [];
              this.selectedCustomer = [];
            }
          }
        );
      }
    }
  }

  viewPriceServiceDetailWitdParam() {
    this.onViewPriceListDetail();
  }

  async AddArea() {
    if (!this.selectedPriceService) {
      this.msgService.error("Vui lòng chọn bảng giá dịch vụ");
      return;
    }

    if (this.areas.length > 25) {
      this.msgService.error("Bảng giá không được vượt quá 25 cột");
      return;
    }

    let dataFilterPriceServiceDetail: DataFilterViewModel = new DataFilterViewModel;
    dataFilterPriceServiceDetail.typeInt1 = this.selectedPriceList;
    dataFilterPriceServiceDetail.typeInt2 = this.selectedService;

    let area = new Area();
    area.areaGroupId = this.priceService.areaGroupId;
    area.code = null;
    area.name = area.code;
    area.isAuto = true;// this.priceServices.find(f => f.value == this.selectedPriceService).data.isAuto;
    area.priceServiceId = this.selectedPriceService;

    await this.areaService.createAsync(area).then(
      x => {
        if (!this.isValidResponse(x)) return;
        this.msgService.success("Thêm cột thành công");
        if (!this.newAreas) this.newAreas = [];
        this.newAreas.push(x.data);
        this.onViewPriceListDetail();
      }
    )
  }

  async addWeight() {
    this.selectedWeight.forEach(async el => {
      if (!this.selectedPriceService) {
        this.msgService.error("Vui lòng chọn bảng giá dịch vụ để thêm dòng");
        return;
      }

      if (this.areaAndSelect.length == 0) {
        this.msgService.error("Vui lòng thêm cột trước");
        return;
      }

      if (Number.parseInt(el.weightFrom) > Number.parseInt(el.weightTo)) {
        this.msgService.error("Giá trị đến phải lớn hơn giá trị từ");
        this.viewPriceServiceDetailWitdParam();
        return;
      }

      if (el.pricingTypeId.value) {
        this.msgService.error("Vui lòng chọn loại tính theo");
        this.viewPriceServiceDetailWitdParam();
        return;
      }

      if (!el.formulaId.value) {
        this.msgService.error("Vui lòng chọn công thức");
        this.viewPriceServiceDetailWitdParam();
        return;
      }
      //
      if (!this.isEditPrice) {
        let weight = new Weight();

        weight.pricingTypeId = el.pricingTypeId.value;//Tính theo
        weight.formulaId = el.formulaId.value;//giá chuẩn
        weight.code = this.priceServices.find(f => f.value == this.selectedPriceService).data.code + '-' + SearchDate.formatToISODate(Date());
        weight.name = weight.code;
        weight.weightFrom = el.weightFrom;
        weight.weightTo = el.weightTo;
        weight.weightGroupId = this.priceServices.find(f => f.value == this.selectedPriceService).data.weightGroupId;
        weight.weightPlus = el.weightPlus | 0;
        weight.isAuto = this.priceServices.find(f => f.value == this.selectedPriceService).data.isAuto;
        weight.isWeightCal = true;
        weight.priceServiceId = this.selectedPriceService;
        this.addwieght_pat;
        weight.structureId = el.structureId;
        this.cloneWeightTo = el.weightTo;

        await this.weightService.createAsync(weight).then(
          x => {
            if (!this.isValidResponse(x)) return;
            this.SavePriceService();
            this.msgService.success("Thêm dòng thành công");
            let newrow = new DataPriceServiceDetails();
            let newareaAndPrics: AreaAndPrice[] = [];
            for (var i = 0; i < this.areaAndSelect.length; i++) {
              let new_areaandprice = new AreaAndPrice;
              let new_priceservicedetail = new PriceServiceDetail;
              new_priceservicedetail.weightId = x.data.id;
              new_priceservicedetail.price = el.areaAndPrices[i].priceBasic.price;;
              new_priceservicedetail.areaId = this.areas[i].id;
              new_priceservicedetail.priceServiceId = this.selectedPriceService;
              new_areaandprice.priceBasic = new_priceservicedetail;
              newareaAndPrics.push(new_areaandprice);
              this.priceServiceDetailService.creatPriceServicesAsync(new_areaandprice.priceBasic);
            }
            newrow.weight = x.data;

            newrow.areaAndPrices = newareaAndPrics;

            this.dataPriceListDetail.push(newrow);
            this.onViewPriceListDetail();
            this.valuefromNum = this.valuetoNum;
            this.valuetoNum = 0;
            // let url = document.getElementById("valueFrom") as HTMLInputElement;

            el.weightFrom = this.cloneWeightTo;
            el.weightTo = 0;
          }
        )
      } else if (this.isEditPrice) {
        el.priceServiceId = this.selectedPriceService;
        const results = await this.weightService.updateAsync(el.weight);

        const updatePriceService = [];
        this.areaAndSelect.map((m, i) => {
          let priceservicedetail = new PriceServiceDetail;
          priceservicedetail.weightId = results.data.id;
          priceservicedetail.price = el.areaAndPrices[i].priceBasic.price;
          priceservicedetail.areaId = el.areaAndPrices[i].area.id;
          priceservicedetail.id = el.areaAndPrices[i].priceBasic.id;
          priceservicedetail.priceServiceId = this.selectedPriceService;
          updatePriceService.push(priceservicedetail);
        });
        if (updatePriceService.length > 0) {
          await this.priceServiceDetailService.updatePriceServicesAsync(updatePriceService);
          this.msgService.success("Sửa dòng thành công");
        }
      }
    })
  }

  async SavePriceService() {

    if (!this.priceService.code) {
      this.msgService.error("Vui lòng nhập mã dịch vụ");
      return;
    }

    if (!this.selectedService) {
      this.msgService.error("Vui lòng chọn dịch vụ");
      return;
    }

    this.priceService.serviceId = this.selectedService;
    this.priceService.dim = this.customerPriceServiceModel.dim;
    this.priceService.remoteAreasPricePercent = this.customerPriceServiceModel.remoteAreasPricePercent;
    this.priceService.fuelPercent = this.customerPriceServiceModel.fuelPercent;
    this.priceService.vatPercent = this.customerPriceServiceModel.vatPercent;

    this.priceService.publicDateFrom = this.selectCreateDateFrom ? moment(this.selectCreateDateFrom).format("YYYY/MM/DD") : null;
    this.priceService.publicDateTo = this.selectCreateDateTo ? moment(this.selectCreateDateTo).format("YYYY/MM/DD") : null;
    const data = await this.priceServiceService.updateAsync(this.priceService);
    if (data) {
      this.msgService.success("Cập nhật bảng giá dịch vụ thành công");
    }

  }

  deletePriceService() {
    this.priceServiceService.deleteAsync(this.priceService).then(
      x => {
        if (!this.isValidResponse(x)) return;
        this.msgService.success("Hủy bảng giá dịch vụ thàng công");
        this.refresCreateprice();
        this.priceService = new PriceService();
      }
    )
  }

  async updateAreaDistricts(item) {
    item.priceServiceId = this.selectedPriceService;
    await this.areaService.updateAreaDistrictsAsync(item);
    this.msgService.success("Cập nhật khu vực giao hàng thành công");
  }

  hideInfo(model: AreaAndSelect) {
    model.showInfo = false;
  }

  showInfo(model: AreaAndSelect) {
    model.showInfo = true;
  }

  confirmDeleteArea() {
    this.areaService.deleteAreaAsync(this.selectedArea).then(
      x => {
        this.onViewPriceListDetail();
        this.msgService.success("Xóa cột thành công");

        this.messageService.add({
          severity: Constant.messageStatus.success,
          detail: 'Xóa cột thành công'
        });
        if (this.newAreas.find(f => f.id == this.selectedArea.id)) {
          this.newAreas.splice(this.newAreas.findIndex(f => f.id == this.selectedArea.id), 1);
        }
        this.selectedArea = null;
      }
    );
  }

  copyPriceService(item: any = null): void {
    this.ref = this.dialogService.open(CoreCopyPriceComponent, {
      header: `${'COPY BẢNG GIÁ'}`,
      width: '30%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.refresCreateprice();
      }
    });
  }

  _addRowWeight(): void {
    if (!this.selectedPriceService) {
      this.msgService.error("Vui lòng chọn bảng giá dịch vụ để thêm dòng");
      return;
    }

    if (this.areaAndSelect.length == 0) {
      this.msgService.error("Vui lòng thêm cột");
      return;
    }
    const itemWeight = this._defaultRowItemWeight();
    this.selectedWeight = this.weights;
    this.selectedWeight.push(itemWeight);
  }

  _defaultRowItemWeight(): DataPriceServiceDetails {
    const itemWeight = new DataPriceServiceDetails();
    if (this.selectedPriceService) {
      itemWeight.weight = new Weight();
      itemWeight.areaAndPrices = [];
      for (const itemArea of this.areaAndSelect) {
        const priceServiceDetail = new PriceServiceDetail();
        const areaAndPrice: AreaAndPrice = new AreaAndPrice();
        areaAndPrice.priceBasic = priceServiceDetail;
        itemWeight.areaAndPrices.push(areaAndPrice);
      }
    } else {
      // itemWeight.weight = new Weight();
      // itemWeight.areaAndPrices = [];
      itemWeight.weight = new Weight();
      itemWeight.areaAndPrices = [];
      for (const itemArea of this.areaAndSelect) {
        const areaAndPrice: AreaAndPrice = new AreaAndPrice();
        itemWeight.areaAndPrices.push(areaAndPrice);
      }
    }

    itemWeight.fakeId = `Id : ${this.weights.length}`;
    return itemWeight;
  }
  // chọn thêm tình thành trong price
  filterFromProvince(event, item: AreaAndSelect, multiFromProvince) {
    if (event.key == "Enter") {
      let value = event.target.value.toLowerCase();
      if (value) {
        let findPro = this.provinces.filter(x => x.label.toLowerCase().indexOf(value) != -1);
        if (findPro.length > 0) {
          this.lstCloneAreaSelect = [];
          var a = JSON.parse(JSON.stringify(this.areaAndSelect));
          this.lstCloneAreaSelect = a;
          let fromProvince = findPro[0];
          let indexSelected = item.multiSelectFromProvince.findIndex(x => x == fromProvince.value);
          if (indexSelected != -1) {
            item.multiSelectFromProvince.splice(indexSelected, 1);
          } else {
            item.multiSelectFromProvince.push(fromProvince.value);
          }
          event.target.setSelectionRange(0, value.length);
          multiFromProvince.updateLabel();
        }
      }
    }
  }

  changeCreateFromProvince(area: AreaAndSelect, multiFromProvince, multiFromDistrict) {
    let areaBefore = this.lstCloneAreaSelect.find(x => x.id == area.id);
    let provinceChange = [];
    area.fromDistricts = area.fromDistricts ? area.fromDistricts : [];
    area.multiSelectFromDistrict = area.multiSelectFromDistrict ? area.multiSelectFromDistrict : [];
    if (area.multiSelectFromProvince.length <= 0) {
      area.filterFromProvince = ` T/T đi đã chọn`;
    } else {
      area.filterFromProvince = `Đã chọn ${area.multiSelectFromProvince.length} T/T đi`;
    }
    if (area.multiSelectFromProvince.length > areaBefore.multiSelectFromProvince.length) {//check
      provinceChange = area.multiSelectFromProvince.filter(x => areaBefore.multiSelectFromProvince.indexOf(x) == -1)
      provinceChange.map(x => {
        this.lstDistricts.filter(ft => ft.data ? ft.data.provinceId == x : false).map(f => {
          area.fromDistricts.push(f);
          area.multiSelectFromDistrict.push(f.value);
        });
      })
    } else {//unCheck
      provinceChange = areaBefore.multiSelectFromProvince.filter(x => area.multiSelectFromProvince.indexOf(x) == -1)
      provinceChange.map(x => {
        area.fromDistricts.filter(f => f.data ? f.data.provinceId == x : false).map(f => {
          area.fromDistricts.splice(area.fromDistricts.findIndex(i => i.value == f.value), 1);
          area.multiSelectFromDistrict.splice(area.multiSelectFromDistrict.findIndex(i => i == f.value), 1);
        });
      })
    }
    if (area.fromDistricts.length <= 0) {
      area.filterFromDistrict = `Chọn Q/H đi`;
    } else {
      area.filterFromDistrict = `Đã chọn ${area.fromDistricts.length} Q/H đi`;
    }
    this.lstCloneAreaSelect = [];
    this.lstCloneAreaSelect = JSON.parse(JSON.stringify(this.areaAndSelect));
    multiFromProvince.updateLabel();
    multiFromDistrict.updateLabel();
    console.log(this.areaAndSelect);
  }

  filterFromDistrictCreate(event, item: AreaAndSelect, multiDistrict, multiProvince) {
    if (event.key == "Enter") {
      let value = event.target.value.toLowerCase();
      if (value) {
        item.fromDistricts = item.fromDistricts.length ? item.fromDistricts : [];
        this.lstCloneAreaSelect = [];
        this.lstCloneAreaSelect = JSON.parse(JSON.stringify(this.areaAndSelect))
        let findDist = item.fromDistricts.filter(x => x.label.toLowerCase().indexOf(value) != -1);
        if (findDist.length > 0) {
          let district = findDist[0];
          let indexSelected = item.multiSelectFromDistrict.findIndex(x => x == district.value);
          if (indexSelected != -1)
            item.multiSelectFromDistrict.splice(indexSelected, 1);
          else
            item.multiSelectFromDistrict.push(district.value);
          event.target.setSelectionRange(0, value.length);
        }
      }
    }
  }

  filterToProvinceCreate(event, item: AreaAndSelect, multiProvince, multiDistrict) {
    if (event.key == "Enter") {
      let value = event.target.value.toLowerCase();
      if (value) {
        let findPro = this.provinces.filter(x => x.label.toLowerCase().indexOf(value) != -1);
        if (findPro.length > 0) {
          this.lstCloneAreaSelect = [];
          var a = JSON.parse(JSON.stringify(this.areaAndSelect));
          this.lstCloneAreaSelect = a;
          let province = findPro[0];
          let indexSelected = item.multiSelectProvinces.findIndex(x => x == province.value);
          if (indexSelected != -1) {
            item.multiSelectProvinces.splice(indexSelected, 1);
          } else {
            item.multiSelectProvinces.push(province.value);
          }
          event.target.setSelectionRange(0, value.length);
          this.changeToProvinceCreate(item, multiProvince, multiDistrict);
          //
        }
      }
    }
  }

  changeToProvinceCreate(area: AreaAndSelect, multiProvince, multiDistrict) {
    let areaBefore = this.lstCloneAreaSelect.find(x => x.id == area.id);
    let provinceChange = [];
    if (area.multiSelectProvinces.length <= 0) {
      area.filterToProvince = ` T/T đến đã chọn`;
    } else {
      area.filterToProvince = `Đã chọn ${area.multiSelectProvinces.length} T/T đến`;
    }

    if (area.multiSelectProvinces.length > areaBefore.multiSelectProvinces.length) {//check
      provinceChange = area.multiSelectProvinces.filter(x => areaBefore.multiSelectProvinces.indexOf(x) == -1)
      provinceChange.map(x => {
        this.lstDistricts.filter(ft => ft.data ? ft.data.provinceId == x : false).map(f => {
          area.districts.push(f);
          area.multiSelectDistrict.push(f.value);
        });
      })
    } else {//unCheck
      provinceChange = areaBefore.multiSelectProvinces.filter(x => area.multiSelectProvinces.indexOf(x) == -1)
      provinceChange.map(x => {
        area.districts.filter(f => f.data ? f.data.provinceId == x : false).map(f => {
          area.districts.splice(area.districts.findIndex(i => i.value == f.value), 1);
          area.multiSelectDistrict.splice(area.multiSelectDistrict.findIndex(i => i == f.value), 1);
        });
      })
    }

    if (area.districts.length <= 0) {
      area.filterToDistrict = `Q/H đến đã chọn`;
    } else {
      area.filterToDistrict = `Đã chọn ${area.districts.length} Q/H đến`;
    }
    this.lstCloneAreaSelect = [];
    this.lstCloneAreaSelect = JSON.parse(JSON.stringify(this.areaAndSelect))
    multiProvince.updateLabel();
    multiDistrict.updateLabel();
  }

  filterDistrict(event, item: AreaAndSelect, multiDistrict, multiProvince) {
    if (event.key == "Enter") {
      let value = event.target.value.toLowerCase();
      if (value) {
        this.lstCloneAreaSelect = [];
        this.lstCloneAreaSelect = JSON.parse(JSON.stringify(this.areaAndSelect))

        let findDist = item.districts.filter(x => x.label.toLowerCase().indexOf(value) != -1);
        if (findDist.length > 0) {
          let district = findDist[0];
          let indexSelected = item.multiSelectDistrict.findIndex(x => x == district.value);
          if (indexSelected != -1)
            item.multiSelectDistrict.splice(indexSelected, 1);
          else
            item.multiSelectDistrict.push(district.value);
          event.target.setSelectionRange(0, value.length);
        }
      }
    }
  }

  async createPriceServices(item: any) {
    this.index = 0;
    let itemCode = item.code;
    this.priceService.code = itemCode;
    await this.priceServiceService.getByCodeAsync(itemCode).then(
      x => {
        this.listPriceService = x as PriceService[];
        this.resultPriceServices = this.listPriceService.map(f => f.code);
      }
    );
    await this.findByCode(itemCode);
    await this.onViewPriceListDetail();
  }
}
