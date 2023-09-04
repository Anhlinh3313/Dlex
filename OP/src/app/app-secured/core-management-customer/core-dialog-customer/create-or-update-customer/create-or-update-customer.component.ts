import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'console';
import { GooglePlaceDirective } from 'ngx-google-places-autocomplete';
import { Address } from 'ngx-google-places-autocomplete/objects/address';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { GMapHelper } from 'src/app/shared/infrastructure/gmap.helper';
import { SearchDate } from 'src/app/shared/infrastructure/searchDate.helper';
import { Customer } from 'src/app/shared/models/entity/customer.model';
import { CustomerPriceList } from 'src/app/shared/models/entity/customerPriceList.model';
import { CustomerType } from 'src/app/shared/models/entity/customerType.model';
import { IconInputModel } from 'src/app/shared/models/entity/icon.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { User } from 'src/app/shared/models/entity/user.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { CustomerService } from 'src/app/shared/services/api/customer.service';
import { CustomerPriceListService } from 'src/app/shared/services/api/customerPriceList.service';
import { CustomerTypeService } from 'src/app/shared/services/api/customerType.service';
import { DistrictService } from 'src/app/shared/services/api/district.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PaymentTypeService } from 'src/app/shared/services/api/paymentType.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { PriceListService } from 'src/app/shared/services/api/priceList.service';
import { ProvinceService } from 'src/app/shared/services/api/province.service';
import { WardService } from 'src/app/shared/services/api/ward.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-or-update-customer',
  templateUrl: './create-or-update-customer.component.html',
  styleUrls: ['./create-or-update-customer.component.scss']
})
export class CreateOrUpdateCustomerComponent extends BaseComponent implements OnInit {
  @ViewChild('placesRef') placesRef: GooglePlaceDirective;
  options = {
    types: [],
    componentRestrictions: { country: 'VN' }
  };

  edit = false;
  customer: Customer = new Customer();
  customerPriceList: CustomerPriceList = new CustomerPriceList();

  customerType: SelectModel[] = [];
  selectedCustomerType: SelectModel;
  paymentTypes: SelectModel[] = [];
  selectedPaymentType: SelectModel;

  provinces: SelectModel[] = [];
  selectedProvince: SelectModel;
  districts: SelectModel[] = [];
  selectedDistrict: SelectModel;
  wards: SelectModel[] = [];
  selectedWard: SelectModel;
  hubs: SelectModel[] = [];
  selectedHub: SelectModel;

  priceList: any[] = [];
  selectedPriceList: SelectModel[] = [];

  selectedPickupUser: any;
  pickupUsers: any[] = [];
  filteredPickupUsers: any[] = [];

  selectedSalesUser: any;
  salesUsers: any[] = [];
  filteredSalesUsers: any[] = [];

  selectedSupportUser: any;
  supportUsers: any[] = [];
  filteredSupportUsers: any[] = [];

  selectedAccountingUser: any;
  accountingUsers: any[] = [];
  filteredAccountingUsers: any[] = [];

  filterViewModel: FilterViewModel = {
    provinceId: null,
    districtid: null,
    wardId: null,
    pageSize: 40,
    pageNumber: 1,
  };
  filterSearchViewModel: FilterViewModel = {
    searchText: '',
    pageSize: 20,
    pageNumber: 1,
  };

  isEnabledCustomer = false;
  setTimer: any;
  timeStopUsing = new Date();
  // ------------- GoogleMaps -------------
  geocoder: any;
  company = environment;
  //
  inputElementNewPassword: IconInputModel = new IconInputModel();
  eyeShow = environment.eyeShow;
  eyeHide = environment.eyeHide;

  constructor(
    protected messageService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    //
    public customerService: CustomerService,
    public customerTypeService: CustomerTypeService,
    public paymentTypeService: PaymentTypeService,
    public priceListService: PriceListService,
    public customerPriceListService: CustomerPriceListService,
    public provinceService: ProvinceService,
    public districtService: DistrictService,
    public wardService: WardService,
    public hubService: HubService,
    public authService: AuthService,
    public zone: NgZone,
  ) {
    super(messageService, router, permissionService);
    if (google.maps.places) {
      // tslint:disable-next-line:new-parens
      this.geocoder = new google.maps.Geocoder;
    }
  }

  ngOnInit(): void {
    this.initData();
  }

  async initData(): Promise<any> {
    await this.getCustomerType();
    await this.getPaymentTypes();
    await this.getProvince();
    await this.getHubs();
    await this.getPriceList();

    if (this.config.data) {
      this.edit = true;
      await this.getById();
      const findProvince = this.provinces.find(f => f.value === this.customer.provinceId);
      if (findProvince) {
        this.selectedProvince = findProvince;
        await this.getDistrictByProvinceId();
        const findDistrict = this.districts.find(f => f.value === this.customer.districtId);
        if (findDistrict) {
          this.selectedDistrict = findDistrict;
          await this.getWardByDistrictId();
          const findWard = this.wards.find(f => f.value === this.customer.wardId);
          if (findWard) {
            this.selectedWard = findWard;
          }
        }
      }
      const findPaymentType = this.paymentTypes.find(f => f.value === this.customer.paymentTypeId);
      if (findPaymentType) {
        this.selectedPaymentType = findPaymentType;
      }
      const findCustomerType = this.customerType.find(f => f.value === this.customer.customerTypeId);
      if (findCustomerType) {
        this.selectedCustomerType = findCustomerType;
      }
      const findHub = this.hubs.find(f => f.value === this.customer.hubId);
      if (findHub) {
        this.selectedHub = findHub;
      }
      if (this.customer.pickupUserId) {
        const res = (await this.authService.getInfoUserById(this.customer.pickupUserId)).data;
        this.pickupUsers.push(res);
        this.selectedPickupUser = `${res.fullName}`;
      }
      if (this.customer.salesUserId) {
        const res = (await this.authService.getInfoUserById(this.customer.salesUserId)).data;
        this.salesUsers.push(res);
        this.selectedSalesUser = `${res.fullName}`;
      }
      if (this.customer.supportUserId) {
        const res = (await this.authService.getInfoUserById(this.customer.supportUserId)).data;
        this.supportUsers.push(res);
        this.selectedSupportUser = `${res.fullName}`;
      }
      if (this.customer.accountingUserId) {
        const res = (await this.authService.getInfoUserById(this.customer.accountingUserId)).data;
        this.accountingUsers.push(res);
        this.selectedAccountingUser = `${res.fullName}`;
      }
      //this.timeStopUsing = new Date(SearchDate.searchFullDate(SearchDate.formatFilterDate(this.customer.timeStopUsing)));

      const resPriceList = await this.customerPriceListService.getPriceListByCustomerId(this.customer.id);
      if (resPriceList.data.length > 0) {
        const customerPriceList = resPriceList.data;
        this.selectedPriceList = customerPriceList.map(m => {
          return { label: `${m.priceList.name}`, value: m.priceListId, data: m.priceList };
        });
      }
      this.isEnabledCustomer = !this.customer.isEnabled;
    }
    this.inputElementNewPassword.src = this.eyeHide;
    this.inputElementNewPassword.type = 'password';
  }

  async getById(): Promise<any> {
    const rep = await this.customerService.getById(this.config.data.id);
    if (rep.isSuccess) {
      this.customer = rep.data;
    }
  }

  async getCustomerType(): Promise<any> {
    this.customerType = await this.customerTypeService.getAllSelectModelAsync();
  }

  async getPaymentTypes(): Promise<any> {
    this.paymentTypes = await this.paymentTypeService.getAllSelectModelAsync();
  }

  async getHubs(): Promise<any> {
    this.hubs = await this.hubService.getAllSelectModelAsync();
  }

  async getPriceList(): Promise<any> {
    this.priceList = await this.priceListService.getAllMultiSelectModelAsync();
  }

  async getProvince(): Promise<any> {
    this.selectedHub = null;
    this.provinces = await this.provinceService.getProvinceAsync();
  }

  async getDistrictByProvinceId(): Promise<any> {
    this.selectedWard = null;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    if (this.selectedProvince.value) {
      this.districts = await this.districtService.getDistrictsSelectModelAsync(this.filterViewModel);
    } else {
      this.districts = [];
    }
    this.wards = [];
  }

  async getWardByDistrictId(): Promise<any> {
    this.selectedWard = null;
    this.filterViewModel.provinceId = this.selectedProvince.value;
    this.filterViewModel.districtid = this.selectedDistrict.value;
    if(this.selectedProvince.value && this.selectedDistrict.value){
      this.wards = await this.wardService.getWardsSelectModelAsync(this.filterViewModel);
    }else {
      this.wards = [];
    }
  }

  async filterAutocomplete(event): Promise<any> {
    let filtered: any[] = [];
    const query = event.query;
    this.filterSearchViewModel.searchText = event.query;
    const rep = await this.authService.searchCodeName(this.filterSearchViewModel);
    if (rep.isSuccess) {
      filtered = rep.data;
    }
    return filtered;
  }

  async filterPickupUser(event): Promise<any> {
    const filtered: any[] = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.pickupUsers = await this.filterAutocomplete(event);
        for (const item of this.pickupUsers) {
          filtered.push(item.fullName);
        }
        this.filteredPickupUsers = filtered;
      } else {
        this.filteredPickupUsers = [];
      }
    }, 1000);
  }

  async filterSalesUser(event): Promise<any> {
    const filtered: any[] = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.salesUsers = await this.filterAutocomplete(event);
        for (const item of this.salesUsers) {
          filtered.push(item.fullName);
        }
        this.filteredSalesUsers = filtered;
      } else {
        this.filteredSalesUsers = [];
      }
    }, 1000);
  }

  async filterSupportUser(event): Promise<any> {
    const filtered: any[] = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.supportUsers = await this.filterAutocomplete(event);
        for (const item of this.salesUsers) {
          filtered.push(item.fullName);
        }
        this.filteredSupportUsers = filtered;
      } else {
        this.filteredSupportUsers = [];
      }
    }, 1000);
  }

  async filterAccountingUser(event): Promise<any> {
    const filtered: any[] = [];
    clearTimeout(this.setTimer);
    this.setTimer = setTimeout(async () => {
      if (event.query.length > 0) {
        this.accountingUsers = await this.filterAutocomplete(event);
        for (const item of this.salesUsers) {
          filtered.push(item.fullName);
        }
        this.filteredAccountingUsers = filtered;
      } else {
        this.filteredAccountingUsers = [];
      }
    }, 1000);
  }

  onSelectPickupUser(): void {
    console.log(this.selectedPickupUser);
  }

  onSelectSalesUser(): void {
    console.log(this.selectedSalesUser);
  }

  onSelectSupportUser(): void {
    console.log(this.selectedSupportUser);
  }

  onSelectAccountingUser(): void {
    console.log(this.selectedAccountingUser);
  }

  async createOrUpdate(): Promise<any> {
    if (!this.isValidData()) { return; }
    if (this.config.data) {
      const customer = await this.mapParam();
      //console.log(customer);
      const res = await this.customerService.update(customer);
      if (res.isSuccess) {
        if (this.selectedPriceList.length > 0) {
          this.selectedPriceList.map(async m => {
            const customerPL = new CustomerPriceList();
            customerPL.customerId = this.customer.id;
            customerPL.priceListId = m.value;
            customerPL.isEnabled = true;

            await this.customerPriceListService.update(customerPL);
          });
          this.closeDialog(true);
        }
        this.messageService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
        this.onClickCancel(true);
      } else {
        let a = Object.values(res.data);
        if (a) {
          this.messageService.error(a.toString() || 'Cập nhật của bạn không thành công!');
        } else {
          this.messageService.error('Cập nhật của bạn không thành công!');
        }
      }
    } else {
      const customer = await this.mapParam();
      const res = await this.customerService.create(customer);
      if (res.isSuccess) {
        const customerUser = new User();
        customerUser.code = customer.code;
        customerUser.userName = customer.userName;
        customerUser.password = customer.passWord;
        customerUser.phoneNumber = customer.phoneNumber;
        customerUser.email = customer.email;
        customerUser.address = customer.address;
        customerUser.typeUserId = 3;
        customerUser.roleId = 89;
        const result = await this.authService.create(customerUser);
        if (result.isSuccess) {
          if (this.selectedPriceList.length > 0) {
            this.selectedPriceList.map(async m => {
              const customerPL = new CustomerPriceList();
              customerPL.customerId = res.data.id;
              customerPL.priceListId = m.value;
              customerPL.isEnabled = true;

              await this.customerPriceListService.create(customerPL);
            });
            this.closeDialog(true);
          }
          this.messageService.success('Tạo mới khách hàng thành công');
          this.onClickCancel(true);
        } else {
          setTimeout(() => {
            this.deleteCustomer(res.data.id)
          }, 1000);
          this.messageService.error(result.message || 'Tạo mới nhân viên thuộc khách hàng không thành công!');
        }
      } else {
        let a = Object.values(res.data);
        if (a) {
          this.messageService.error(a.toString() || 'Tạo mới khách hàng không thành công!');
        } else {
          this.messageService.error('Tạo mới khách hàng không thành công!');
        }
      }
    }
  }

  async deleteCustomer(item) {
    let customerId = item;
    const updateCustomer = await this.customerService.updateCustomerbyUserFail(customerId);
    if (updateCustomer.data[0].isSuccess) {
    } else {
      this.messageService.error(updateCustomer.data[0].message || 'Cập nhật của bạn không thành công!');
    }
  }

  async mapParam(): Promise<any> {
    let customerPram = new Customer();

    customerPram = JSON.parse(JSON.stringify(this.customer));

    customerPram.provinceId = this.selectedProvince.value;
    customerPram.districtId = this.selectedDistrict.value;
    customerPram.wardId = this.selectedWard.value;
    if (this.selectedHub) {
      customerPram.hubId = this.selectedHub.value;
    }

    customerPram.customerTypeId = this.selectedCustomerType ? this.selectedCustomerType.value : null;
    customerPram.paymentTypeId = this.selectedPaymentType ? this.selectedPaymentType.value : null;

    const findPickupUser = this.pickupUsers.find(f => f.fullName === this.selectedPickupUser);
    if (findPickupUser) {
      this.customer.pickupUserId = findPickupUser.id;
    }
    const findSalesUser = this.salesUsers.find(f => f.fullName === this.selectedSalesUser);
    if (findSalesUser) {
      this.customer.salesUserId = findSalesUser.id;
    }
    const findSupportUser = this.supportUsers.find(f => f.fullName === this.selectedSupportUser);
    if (findSupportUser) {
      this.customer.supportUserId = findSupportUser.id;
    }
    const findAccountingUser = this.accountingUsers.find(f => f.fullName === this.selectedAccountingUser);
    if (findAccountingUser) {
      this.customer.accountingUserId = findAccountingUser.id;
    }

    customerPram.pickupUserId = this.customer.pickupUserId;
    customerPram.salesUserId = this.customer.salesUserId;
    customerPram.supportUserId = this.customer.supportUserId;
    customerPram.accountingUserId = this.customer.accountingUserId;
    customerPram.userName = this.customer.userName;
    customerPram.email = this.customer.email;
    if(!this.config.data){
      const code = await this.customerService.ranDomCodeCustomer(this.company.customerCode)
      if (code.isSuccess) {
        customerPram.code = code.data[0].customerCode;
      }
    } else {
      customerPram.code = this.config.data.code
    }
    //this.customer.timeStopUsing = SearchDate.formatToISODate(this.timeStopUsing);
    this.customer.isEnabled = !this.isEnabledCustomer;

    return customerPram;
  }

  onChangeTest(): void {
    console.log(this.selectedPriceList);
  }

  closeDialog(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  async handleAddressChange(address: Address, inputAddress): Promise<any> {
    this.selectedAddressItem(address, inputAddress);
  }

  selectedAddressItem(prediction, inputAddress): void {
    if (inputAddress.id === 'address') {
      this.customer.address = prediction.formatted_address;
    }
    this.geocoder.geocode({ placeId: prediction.place_id }, (resultsPlaceId, statusPlaceId) => {
      if (statusPlaceId === 'OK' && resultsPlaceId[0]) {
        const latlng = resultsPlaceId[0].geometry.location;
        this.geocoder.geocode({ placeId: resultsPlaceId[0].place_id }, (resultsLatLng, statusLatLng) => {
          if (statusLatLng === 'OK' && resultsLatLng[0]) {
            this.loadInitMap(resultsLatLng, inputAddress);
          }
        });
      }
    });
  }

  async loadInitMap(resultsLatLng, inputAddress): Promise<any> {
    const results = resultsLatLng[0];
    const lat = results.geometry.location.lat();
    const lng = results.geometry.location.lng();
    this.selectedHub = null;
    let provinceName; let districtName; let wardName;
    results.address_components.map(element => {
      //
      if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_1) !== -1) {
        provinceName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) !== -1) {
        districtName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.LOCALITY) !== -1
        && element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_2) === -1) {
        districtName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.ADMINISTRATIVE_AREA_LEVEL_3) !== -1) {
        wardName = element.long_name;
      } else if (element.types.indexOf(GMapHelper.SUBLOCALITY_LEVEL_1) !== -1) {
        wardName = element.long_name;
      }
    });

    if (inputAddress.id === 'address') {
      await this.zone.run(async () => {
        const res = await this.hubService.getInfoLocation(provinceName, districtName, wardName);
        if (res) {
          const findProvince = this.provinces.find(f => f.value === res.provinceId);
          if (findProvince) {
            this.selectedProvince = findProvince;
            await this.getDistrictByProvinceId();
            const findDistrict = this.districts.find(f => f.value === res.districtId);
            if (findDistrict) {
              this.selectedDistrict = findDistrict;
              await this.getWardByDistrictId();
              const findWard = this.wards.find(f => f.value === res.wardId);
              if (findWard && wardName) {
                this.selectedWard = findWard;
                this.getHubByAddress();
              }
            }
            // if (res.hubId) {
            //   this.customer.hubId = res.hubId;
            // }
          }
          this.customer.provinceId = res.provinceId;
          this.customer.districtId = res.districtId;
          this.customer.wardId = res.wardId;
          this.customer.lat = lat;
          this.customer.lng = lng;
        }
      });
    }
  }

  async getHubByAddress() {
    const resHub = await this.hubService.getHubByProvinceDistrictWardSelectModelAsync(this.selectedProvince.value, this.selectedDistrict.value, this.selectedWard.value);
    if (resHub.length > 1 && this.selectedWard.value) {
      if (resHub.length == 2) {
        this.hubs = resHub;
        this.selectedHub = this.hubs[1];
      }
      if (resHub.length > 2) {
        this.hubs = resHub;
      }
    }
    else {
      this.hubs = [];
    }
  }

  isValidData(): boolean {

    // if (!this.customer.code) {
    //   this.messageService.error('Vui lòng nhập mã khách hàng');
    //   return false;
    // }

    if (!this.customer.userName) {
      this.messageService.error('Vui lòng nhập tài khoản đăng ký');
      return false;
    }

    if(!this.config.data){
      if (!this.customer.passWord) {
        this.messageService.error('Vui lòng nhập mật khẩu');
        return false;
      }
    }

    if (!this.customer.name) {
      this.messageService.error('Vui lòng nhập tên');
      return false;
    }

    if (!this.customer.phoneNumber) {
      this.messageService.error('Vui lòng nhập điện thoại');
      return false;
    }

    if (!this.customer.email) {
      this.messageService.error('Vui lòng nhập email');
      return false;
    }

    if (!this.customer.address) {
      this.messageService.error('Vui lòng nhập địa chỉ lấy hàng');
      return false;
    }

    if (!this.selectedProvince) {
      this.messageService.error('Vui lòng chọn tỉnh thành');
      return false;
    }

    if (!this.selectedDistrict) {
      this.messageService.error('Vui lòng chọn quận huyện');
      return false;
    }

    if (!this.selectedWard) {
      this.messageService.error('Vui lòng chọn Phường/Xã');
      return false;
    }

    // if (!this.selectedHub) {
    //   this.messageService.error('Vui lòng chọn TT/CN/T Nhận');
    //   return false;
    // }

    return true;
  }

  onClickCancel(event): void {
    if (this.ref) {
      this.ref.close(event);
    }
  }

  onClickNewPassword() {
    if (this.inputElementNewPassword.src === this.eyeShow) {
      this.inputElementNewPassword.src = this.eyeHide;
      this.inputElementNewPassword.type = 'password';
    } else if (this.inputElementNewPassword.src === this.eyeHide) {
      this.inputElementNewPassword.src = this.eyeShow;
      this.inputElementNewPassword.type = 'text';
    }
  }

}
