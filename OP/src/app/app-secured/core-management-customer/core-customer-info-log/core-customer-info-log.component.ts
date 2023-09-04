import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ObjectHelper } from 'src/app/infrastructure/enums/object.helper';
import { BaseComponent, ColumnExcel } from 'src/app/shared/components/baseComponent';
import { CustomerInfoLog } from 'src/app/shared/models/entity/customerInfoLog.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { CustomerInfoLogService } from 'src/app/shared/services/api/customerInfoLog.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { BreadcrumbService } from 'src/app/shared/services/local/breadcrumb.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { CreateOrUpdateCustomerInfoLogComponent } from '../core-dialog-customer/create-or-update-customer-info-log/create-or-update-customer-info-log.component';
import { LazyLoadEvent } from 'primeng/api';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { MapsAPILoader } from '@agm/core';
import { InfoLocation } from 'src/app/shared/models/entity/infoLocation.model';
import { GMapHelper } from 'src/app/shared/infrastructure/gmap.helper';
import { GeocodingApiService } from 'src/app/shared/services/api/geocodingApiService.service';
type AOA = Array<Array<any>>;

@Component({
  selector: 'app-core-customer-info-log',
  templateUrl: './core-customer-info-log.component.html',
  styleUrls: ['./core-customer-info-log.component.scss']
})
export class CoreCustomerInfoLogComponent extends BaseComponent implements OnInit {

  @ViewChild("inputFiles") inputFilesVariable: any;
  @ViewChild('ShippingAddress') shippingAddressElementRef: ElementRef;
  rows = 20;
  first = 0;
  dialogDelete = false;
  ref: DynamicDialogRef;
  filterViewModel: FilterViewModel;
  totalRecords: number;
  searchText: any;
  onPageChangeEvent: any;
  selectedData: any;
  //data
  customerInfoLog: CustomerInfoLog[] = [];
  customerInfoLogs: CustomerInfoLog[] = [];

  province: SelectModel[] = [];
  selectProvince: SelectModel;
  targetDataTransfer: DataTransfer;
  //ex
  totalRecordExe = 0;
  fileToUpload: File = null;
  arrayBuffer: any;
  customerInfoLogFileExcel: CustomerInfoLog[] = [];
  customerInfoLogLoading = false;
  customerInfoLogEvent: LazyLoadEvent;
  selectedCustomerInfoLog: CustomerInfoLog;
  dataSourceCustomerInfoLog: CustomerInfoLog[] = [];
  districts: SelectModel[] = [];
  wards: SelectModel[] = [];
  selectedWard: SelectModel;
  displayConfirmCreate: boolean;
  @ViewChild('inputFile') inputFileElementRef: ElementRef;
  provinceExeId: number;
  districtExeId: number;
  roleLoading : boolean = false;

  constructor(
    protected wardService: WardService,
    protected districtService: DistrictService,
    protected dialogService: DialogService,
    protected provinceService: ProvinceService,
    protected customerInfoLogService: CustomerInfoLogService,
    protected breadcrumbService: BreadcrumbService,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    public ngZone: NgZone,
    protected mapsAPILoader: MapsAPILoader,
    public hubService: HubService,
    protected geocodingApiService: GeocodingApiService,
  ) {
    super(msgService, router, permissionService);
    this.breadcrumbService.setItems([
      { label: 'Quản lý khách hàng' },
      { label: 'Thông tin khách nhận' }
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
    this.getProvince();
    this.loadCustomerInfoLog();
    this.dataSourceCustomerInfoLog = null;
  }

  async getProvince(): Promise<any> {
    this.province = await this.provinceService.getProvinceAsync();
  }

  async loadCustomerInfoLog() {
    this.roleLoading = true;
    const results = await this.customerInfoLogService.getListCustomerInfoLogr(this.filterViewModel);
    if (results.data.length > 0) {
      this.customerInfoLog = results.data;
      this.totalRecords = this.customerInfoLog[0].totalCount || 0;
      this.roleLoading = false;
    } else {
      this.customerInfoLog = [];
      this.totalRecords = 0;
      this.first = -1;
      this.roleLoading = false;
    }
  }

  createCustomerInfoLog(item: any = null): void {
    this.ref = this.dialogService.open(CreateOrUpdateCustomerInfoLogComponent, {
      header: `${item ? 'SỬA KHÁCH HÀNG NHẬN' : 'TẠO MỚI KHÁCH HÀNG NHẬN'}`,
      width: '40%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: ObjectHelper.clone(item),
    });
    this.ref.onClose.subscribe((res: any) => {
      if (res) {
        this.loadCustomerInfoLog();
      }
    });
  }

  changeProvince() {
    this.filterViewModel.provinceId = this.selectProvince.value;
    this.loadCustomerInfoLog();
  }

  refresher() {
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.pageSize = 20;
    this.filterViewModel.searchText = null;
    this.filterViewModel.provinceId = null;
    this.searchText = null;
    this.selectProvince = null;
    this.loadCustomerInfoLog();
  }

  onFilter() {
    this.first = 0;
    this.filterViewModel.pageNumber = 1;
    this.filterViewModel.searchText = this.searchText.trim();
    this.loadCustomerInfoLog();
  }

  onPageChange(event: any): void {
    this.onPageChangeEvent = event;
    this.filterViewModel.pageNumber = this.onPageChangeEvent.first / this.onPageChangeEvent.rows + 1;
    this.filterViewModel.pageSize = this.onPageChangeEvent.rows;
    this.loadCustomerInfoLog();
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  async deleteCustomerInfoLog() {
    const res = await this.customerInfoLogService.delete(this.selectedData);
    if (res.isSuccess) {
      this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
      this.dialogDelete = false;
      this.loadCustomerInfoLog();
    } else {
      this.msgService.error(res.message || 'Cập nhật của bạn không thành công!');
    }
  }

  async onUpload(files: FileList) {
    this.fileToUpload = files.item(0);
    const fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.arrayBuffer = fileReader.result;
      const data = new Uint8Array(this.arrayBuffer);
      const arr = new Array();
      for (let i = 0; i !== data.length; ++i) { arr[i] = String.fromCharCode(data[i]); }
      const bstr = arr.join('');
      const workbook = XLSX.read(bstr, { type: 'binary' });
      const sheetName = workbook.SheetNames[0];
      const worksheet = workbook.Sheets[sheetName];
      this.customerInfoLogFileExcel = XLSX.utils.sheet_to_json(worksheet, { raw: true });
      this.initData();
    };
    fileReader.readAsArrayBuffer(this.fileToUpload);
  }

  async initData() {
    if (this.customerInfoLogFileExcel.length > 1) {
      this.customerInfoLogs = [];
      this.totalRecordExe = this.customerInfoLogFileExcel.length - 1;
      for (const key in this.customerInfoLogFileExcel) {
        if (Object.prototype.hasOwnProperty.call(this.customerInfoLogFileExcel, key) && Number(key) > 0) {
          this.customerInfoLogLoading = true;
          setTimeout(() => {
            this.customerInfoLogLoading = false;
          }, 10000);
          const el = this.customerInfoLogFileExcel[key];
          let adress = await this.geocodingApiService.findFromAddressAsync(el.address);
          let placesFirst = adress as google.maps.places.PlaceResult;
          let lat = placesFirst.geometry.location.lat();
          let lng = placesFirst.geometry.location.lng();
          let placesFirsts = await this.geocodingApiService.findFirstFromLatLngAsync(lat, lng) as google.maps.places.PlaceResult;
          const locationInfo = await this.getLocationByPlacesResult(placesFirsts);
          const resultCustomerInfoLog = await this.mapingcustomerInfoLog(el, key, locationInfo);
          if (Object.keys(resultCustomerInfoLog.isError).length > 0) {
            this.customerInfoLogs.unshift(resultCustomerInfoLog);
          } else {
            this.customerInfoLogs.push(resultCustomerInfoLog);
          }
          await this.loadcustomerInfoLogLazy(this.customerInfoLogEvent);
        }
      }
      this.customerInfoLogLoading = false;
    }
  }

  loadcustomerInfoLogLazy(event: LazyLoadEvent, isHtmlLoading?: boolean) {
    if (isHtmlLoading) {
      this.customerInfoLogLoading = true;
    }
    setTimeout(() => {
      if (this.customerInfoLogs && event) {
        this.customerInfoLogEvent = event;
        this.dataSourceCustomerInfoLog = this.customerInfoLogs.slice(event.first, (event.first + event.rows));
        if (isHtmlLoading) {
          this.customerInfoLogLoading = false;
        }
      }
    }, 0);
  }

  async mapingcustomerInfoLog(customerInfoLogModel: CustomerInfoLog, index: any, locationInfo: any) {
    const customerInfoLog = new CustomerInfoLog();
    //
    customerInfoLog.fakeId = Number(index);
    customerInfoLog.code = customerInfoLogModel.code;
    customerInfoLog.name = customerInfoLogModel.name ? customerInfoLogModel.name : null;
    customerInfoLog.phoneNumber = customerInfoLogModel.phoneNumber ? customerInfoLogModel.phoneNumber : null;
    customerInfoLog.address = customerInfoLogModel.address ? customerInfoLogModel.address : null;
    customerInfoLog.provinceName = customerInfoLogModel.provinceName ? customerInfoLogModel.provinceName : null;
    customerInfoLog.addressNote = customerInfoLogModel.addressNote ? customerInfoLogModel.addressNote : null;
    customerInfoLog.provinceId = locationInfo.provinceId ? locationInfo.provinceId : null;
    customerInfoLog.districtId = locationInfo.districtId ? locationInfo.districtId : null;
    customerInfoLog.wardId = locationInfo.wardId ? locationInfo.wardId : null;
    return customerInfoLog;
  }

  resetExcel() {
    this.customerInfoLogLoading = false;
    // this.inputFileElementRef.nativeElement.value = '';
    this.customerInfoLogs = [];
    this.dataSourceCustomerInfoLog = [];
    this.inputFilesVariable.nativeElement.value = "";
  }

  async onChangeProvinceExe(item) {
    this.provinceExeId = item.provinceId;
    item.districtName = null;
    item.districtId = null;
    item.wardName = null;
    item.wardId = null;
    let findCustomerInfoLog = this.customerInfoLogs.find(f => f.fakeId === item.fakeId);
    if (findCustomerInfoLog) {
      const findprovince = this.province.find(f => f.value === item.provinceId);
      if (findprovince) {
        findCustomerInfoLog.provinceId = findprovince.value;
        findCustomerInfoLog.provinceName = findprovince.label;
      }
    }
    await this.getDistrictByProvinceId();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.filterViewModel.provinceId = this.provinceExeId;
    this.districts = await this.districtService.getDistrictsSelectModelAsync(this.filterViewModel);
  }

  async getWardByDistrictId(): Promise<any> {
    this.filterViewModel.provinceId = this.provinceExeId;
    this.filterViewModel.districtid = this.districtExeId;
    this.wards = await this.wardService.getWardsSelectModelAsync(this.filterViewModel)
  }

  async onClickCreateCustomerInfoLog() {
    this.displayConfirmCreate = false;
    this.customerInfoLogLoading = true;
    let countError = 0;
    const customerInfoLogErrorgError: CustomerInfoLog[] = [];
    const customerInfoLogErrorNormal: CustomerInfoLog[] = [];

    for (const key in this.customerInfoLogs) {
      if (Object.prototype.hasOwnProperty.call(this.customerInfoLogs, key)) {
        const el = this.customerInfoLogs[key];
        const customerInfoLog = await this.isValidateShipment(el);
        if (Object.keys(customerInfoLog.isError).length > 0) {
          countError++;
          customerInfoLogErrorgError.push(customerInfoLog);
        } else {
          customerInfoLogErrorNormal.push(el);
        }
      }
    }
    
    this.customerInfoLogs = [];
    this.customerInfoLogs = customerInfoLogErrorgError.concat(customerInfoLogErrorNormal);
    this.loadcustomerInfoLogLazy(this.customerInfoLogEvent);
    if (countError > 0) {
      this.customerInfoLogLoading = false;
      this.msgService.error(`Lỗi ${countError} khách nhận / tổng ${this.customerInfoLogs.length} khách nhận`);
      return;
    }
    const resultUpload = await this.customerInfoLogService.createOrUpdateImportExcel(this.customerInfoLogs);
    this.msgService.success(resultUpload.message);
    this.customerInfoLogs = [];
    this.customerInfoLogLoading = false;
    this.dataSourceCustomerInfoLog = [];
    this.loadcustomerInfoLogLazy(this.customerInfoLogEvent);
    this.inputFileElementRef.nativeElement.value = '';
  }

  isValidateShipment(customerInfoLog: CustomerInfoLog) {
    const patternPhone = /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/;
    if (!customerInfoLog.code) {
      customerInfoLog.isError.isErrorCode = true;
      customerInfoLog.isError.messageisErrorCode = 'Mã khách nhận đang để trống!';
    } else {
      delete customerInfoLog.isError.isErrorCode;
      delete customerInfoLog.isError.messageisErrorCode;
    }

    if (!customerInfoLog.name) {
      customerInfoLog.isError.isErrorName = true;
      customerInfoLog.isError.messageName = 'Tên khách nhận đang để trống!';
    } else {
      delete customerInfoLog.isError.isErrorName;
      delete customerInfoLog.isError.messageName;
    }

    if (!customerInfoLog.phoneNumber) {
      customerInfoLog.isError.isErrorphoneNumber = true;
      customerInfoLog.isError.messagephoneNumber = 'Số điện thoại đang để trống!';
    } else {
      delete customerInfoLog.isError.isErrorphoneNumber;
      delete customerInfoLog.isError.messagephoneNumber;
    }

    if (!patternPhone.test(customerInfoLog.phoneNumber)) {
      customerInfoLog.isError.isErrorphoneNumber = true;
      customerInfoLog.isError.messagephoneNumber = 'Số điện thoại không đúng định dạng!';
    } else {
      delete customerInfoLog.isError.isErrorphoneNumber;
      delete customerInfoLog.isError.messagephoneNumber;
    }


    if (!customerInfoLog.address) {
      customerInfoLog.isError.isErroraddress = true;
      customerInfoLog.isError.messageaddress = 'địa chỉ đang để trống!';
    } else {
      delete customerInfoLog.isError.isErroraddress;
      delete customerInfoLog.isError.messageaddress;
    }

    if (!customerInfoLog.provinceId) {
      customerInfoLog.isError.isErrorProvince = true;
      customerInfoLog.isError.messageProvince = 'Tỉnh thành đang để trống!';
    } else {
      delete customerInfoLog.isError.isErrorProvince;
      delete customerInfoLog.isError.messageProvince;
    }
    return customerInfoLog;
  }

  hasObject(model) {
    if (Object.keys(model).length > 0) {
      return true;
    }
    return false;
  }

  onClickShippingAddress(item) {
    setTimeout(() => {
      this.initMap(item);
    }, 5e2);
  }

  initMap(item) {
    // set google maps defaults
    const elObjs = document.getElementsByClassName('pac-container');
    for (const key in elObjs) {
      if (Object.prototype.hasOwnProperty.call(elObjs, key)) {
        const el = elObjs[key];
        el.remove();
      }
    }
    // load Places Autocomplete
    this.mapsAPILoader.load().then(() => {
      const fromAutocomplete = new google.maps.places.Autocomplete(
        this.shippingAddressElementRef.nativeElement,
        {
          // types: ["address"]
        }
      );
      fromAutocomplete.addListener('place_changed', () => {
        this.ngZone.run(async () => {
          // get the place result
          const placeResult: google.maps.places.PlaceResult = fromAutocomplete.getPlace();
          // verify result
          let locationInfo = new InfoLocation();
          const geocoder = new google.maps.Geocoder();
          geocoder.geocode({ location: placeResult.geometry.location }, async (results, status) => {
            if (status.toString() === 'OK' && results[0]) {
              locationInfo = await this.getLocationByGeocoderResult(results);
              const findShipment = this.customerInfoLogs.find(f => f.fakeId === item.fakeId);
              if (findShipment) {
                findShipment.address = placeResult.formatted_address;
                if (locationInfo) {
                  findShipment.provinceId = locationInfo.provinceId;
                  findShipment.provinceName = locationInfo.provinceName;
                  findShipment.districtId = locationInfo.districtId;
                  findShipment.districtName = locationInfo.districtName;
                  findShipment.wardId = locationInfo.wardId;
                  findShipment.wardName = locationInfo.wardName;
                } else {
                  findShipment.provinceId = null;
                  findShipment.provinceName = '';
                  findShipment.districtId = null;
                  findShipment.districtName = '';
                  findShipment.wardId = null;
                  findShipment.wardName = '';
                }
              }
            } else {
              this.msgService.error('Không tìm thấy địa chỉ');
            }
          });
        });
      });
    });
  }

  async getLocationByGeocoderResult(geocoderResult: google.maps.GeocoderResult[]) {
    let results = geocoderResult.find(f => f.types[0] === 'administrative_area_level_3');
    if (!results) {
      results = geocoderResult.find(f => f.types[2] === 'sublocality_level_1');
    }
    const infoLocation = new InfoLocation();
    if (results) {
      results.address_components.map(element => {
        //
        if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
          infoLocation.provinceName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1) {
          infoLocation.wardName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
          infoLocation.wardName = element.long_name;
        }
      });
    }
    const locationInfo = await this.hubService.getInfoLocation(infoLocation.provinceName, infoLocation.districtName, infoLocation.wardName);
    return locationInfo;
  }
  async getLocationByPlacesResult(geocoderResult: google.maps.places.PlaceResult) {
    const infoLocation = new InfoLocation();
    if (geocoderResult) {
      geocoderResult.address_components.map(element => {
        //
        if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
          infoLocation.provinceName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1) {
          infoLocation.districtName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1) {
          infoLocation.wardName = element.long_name;
        }
        else if (element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
          infoLocation.wardName = element.long_name;
        }
      });
    }
    const locationInfo = await this.hubService.getInfoLocation(infoLocation.provinceName, infoLocation.districtName, infoLocation.wardName);
    return locationInfo;
  }
}
