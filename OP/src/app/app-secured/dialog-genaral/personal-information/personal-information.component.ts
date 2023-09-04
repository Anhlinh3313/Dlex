import { MapsAPILoader } from '@agm/core';
import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { SortUtil } from 'src/app/shared/infrastructure/sort.util';
import { IconInputModel } from 'src/app/shared/models/entity/icon.model';
import { InfoLocation } from 'src/app/shared/models/entity/infoLocation.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { User } from 'src/app/shared/models/entity/user.model';
import { UserRole } from 'src/app/shared/models/entity/userRole.model';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { DepartmentService } from 'src/app/shared/services/api/department.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss']
})
export class PersonalInformationComponent implements OnInit {
  @ViewChild('Address') shippingAddressElementRef: ElementRef;
  userItem: User = new User();
  userRoles: UserRole[] = [];
  //
  manageHubs: SelectModel[] = [];
  hubs: SelectModel[] = [];
  departments: SelectModel[] = [];
  roles: SelectModel[] = [];
  selectedManageHub: SelectModel;
  selectedHub: SelectModel;
  selectedRole: SelectModel;
  selectedRoles: SelectModel[] = [];
  selectedDepartment: SelectModel;
  manageHub = '1';
  inputElementNewPassword: IconInputModel = new IconInputModel();
  eyeShow = environment.eyeShow;
  eyeHide = environment.eyeHide;
  isEdit = true;

  listHub: any = [];
  locationHref = location.pathname;
  centerHubs: any = [];
  poHubs: any = [];
  stationHubs: any = [];
  // Đã chọn {0} chức vụ
  selectedItemsLabel: string;
  isAdd = true;
  isDelete = true;
  checkIsEdit = true;

  constructor(
    private authService: AuthService,
    private roleService: RoleService,
    private hubService: HubService,
    private departmentService: DepartmentService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    protected msgService: MsgService,
    protected router: Router,
    protected permissionService: PermissionService,
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,
    //
  ) {
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData(): Promise<any> {
    await this.loadCheckRole();
    this.selectedItemsLabel = `Đã chọn ${0} chức vụ`;
    await this.loadManageHub();
    await this.loadRole();
    let userId = parseInt(localStorage.getItem(Constant.auths.userId))
    const result = await this.authService.getInfoUserById(userId);
    const checkRoleId = result.data.roles.find(f => f.id === 1);
    this.userItem = result.data;
    this.userRoles = this.userItem.userRoleIds;
    if (this.userItem) {
      if(userId == 1 || checkRoleId){
        this.checkIsEdit = false;
      }
    }
    // this.findRoleData();
    await this.findUserRolesData();
    await this.findManageHubData();
    await this.loadHubByManageHubId();
    await this.findHubData();
    this.inputElementNewPassword.src = this.eyeHide;
    this.inputElementNewPassword.type = 'password';
    this.initMap();
  }

  async loadCheckRole() {
    await this.permissionService.checkPermissionDetail("/core-general/users", 1).then(x => {
      if (x.isSuccess) {
        if (x.data["length"] > 0) {
          let data = x.data[0];
          if (!data.access) {
            this.router.navigate(["403"]);
          }
          else {
            this.isAdd = data.add;
            this.isDelete = data.delete;
            this.isEdit = data.edit;
          }
        }
      }
    });
  }

  async findManageHubData(): Promise<any> {
    const userHub = this.listHub.find(f => f.id === this.userItem.manageHubId);
    if (userHub) {
      if (!userHub.centerHubId) {
        this.manageHub = '1';
      } else if (userHub.centerHubId && !userHub.poHubId) {
        this.manageHub = '2';
      } else if (userHub.centerHubId && userHub.poHubId) {
        this.manageHub = '3';
      }
      await this.loadManageHub();
      const findHub = this.manageHubs.find(f => f.value === userHub.id);
      this.selectedManageHub = findHub;
    }
  }

  findHubData(): void {
    const findHub = this.hubs.find(f => f.value === this.userItem.hubId);
    if (findHub) {
      this.selectedHub = findHub;
    }
  }

  // findRoleData(): void {
  //   const findRole = this.roles.find(f => f.value === this.userItem.roleId);
  //   if (findRole) {
  //     this.selectedRole = findRole;
  //   }
  // }

  findUserRolesData(): void {
    this.userRoles.forEach(fe => {
      const findRole = this.roles.find(f => f.value === fe.roleId);
      if (findRole) {
        this.selectedRoles.push(findRole);
      }
    });

    this.selectedItemsLabel = `Đã chọn ${this.selectedRoles.length} chức vụ`;
    // const findRole = this.roles.find(f => f.value === this.userItem.roleId);
    // if (findRole) {
    //   this.selectedRole = findRole;
    // }
  }

  onChangeManageHub(): void {
    this.userItem.manageHubId = this.selectedManageHub.value;
    this.loadHubByManageHubId()
  }

  onChangeHub(): void {
    this.userItem.hubId = this.selectedHub.value;
  }

  onChangeDepartment(): void {
    this.userItem.departmentId = this.selectedDepartment.value;
  }

  // onChangeRole(): void {
  //   this.userItem.roleId = this.selectedRole.value;
  // }

  onChangeUserRoles(): void {
    this.userItem.roleIds = [];
    this.selectedRoles.map(m => {
      if (m.value) {
        this.userItem.roleIds.push(m.value);
      }
    });
    this.selectedItemsLabel = `Đã chọn ${this.selectedRoles.length} chức vụ`;
  }

  onPageChange(event): void {
  }

  onClick(): void {
    this.msgService.msgBoxSuccess('Cập nhật của bạn đã được thay đổi trên hệ thống');
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  async getDropdownHub(arr, type): Promise<SelectModel[]> {
    const selectModel = [];
    let data = [];
    if (arr) {
      switch (type) {
        case 1: data = this.listHub.filter(x => x.centerHubId == null);
          break;
        case 2: data = this.listHub.filter(x => x.centerHubId && x.poHubId == null);
          break;
        case 3: data = this.listHub.filter(x => x.centerHubId && x.poHubId);
          break;
      }

      data = SortUtil.sortAlphanumerical(data, 1, 'code');
      data.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: '-- Chọn dữ liệu chi nhánh --', data: null, value: null });
      return selectModel;
    }
  }

  async loadManageHub(): Promise<any> {
    const listHub = await this.hubService.getAll() as any;
    if (listHub.data && listHub.data.length > 0) {
      this.listHub = listHub.data;
      this.centerHubs = this.getDropdownHub(this.listHub, 1);
      this.poHubs = this.getDropdownHub(this.listHub, 2);
      this.stationHubs = this.getDropdownHub(this.listHub, 3);
    }

    this.loadHubCenter();
  }

  loadCheckRoleEdit() {
    if (!this.checkIsEdit) {
      this.loadHubCenter();
    }
  }

  async loadHubCenter(): Promise<any> {
    this.selectedManageHub = null;
    if (this.manageHub === '1') {
      this.manageHubs = await this.centerHubs;
      this.hubs = null;
    }
    if (this.manageHub === '2') {
      this.manageHubs = await this.poHubs;
      this.hubs = null;
    }
    if (this.manageHub === '3') {
      this.manageHubs = await this.stationHubs;
      this.hubs = null;
    }
  }

  async loadHubByManageHubId(): Promise<any> {
    this.hubs = await this.hubService.getListHubFromHubIdUserSelectModelAsync(this.selectedManageHub.value);
  }

  async loadRole(): Promise<any> {
    this.roles = await this.roleService.getRolesMultiSelectAsync();
  }

  onClickSave(): void {
    this.updateUser();
  }

  async updateUser(): Promise<any> {
    if (!this.isValidDataUpdate()) { return; }
    this.userItem.isEnabled = true;
    this.userItem.roleIds = [];
    this.selectedRoles.map(m => {
      if (m.value) {
        this.userItem.roleIds.push(m.value);
      }
    });
    const res = await this.authService.updateUser(this.userItem);
    if (res.isSuccess) {
      this.msgService.msgBoxSuccess('Cập nhật tài khoản thành công');
      let locationHrefLength = this.locationHref.search("/core-general/users");
      if (locationHrefLength >= 0) {
        window.location.reload(true);
      }
      this.onClickCancel(true);
    } else {
      this.msgService.msgBoxError(res.message);
    }
  }

  onClickNewPassword(): void {
    if (this.inputElementNewPassword.src === this.eyeShow) {
      this.inputElementNewPassword.src = this.eyeHide;
      this.inputElementNewPassword.type = 'password';
    } else if (this.inputElementNewPassword.src === this.eyeHide) {
      this.inputElementNewPassword.src = this.eyeShow;
      this.inputElementNewPassword.type = 'text';
    }
  };

  isValidDataUpdate(): boolean {
    const patternPhone = /((03|05|07|08|09)[0-9])+([0-9]{7})\b/;
    const patternEmail = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!this.userItem.code) {
      this.msgService.msgBoxError('Vui lòng nhập mã tài khoản');
      return false;
    }

    if (!this.userItem.userName) {
      this.msgService.msgBoxError('Vui lòng nhập tên');
      return false;
    }

    if (!this.userItem.fullName) {
      this.msgService.msgBoxError('Vui lòng nhập đầy đủ họ tên');
      return false;
    }

    if (!this.userItem.phoneNumber) {
      this.msgService.msgBoxError('Vui lòng nhập số điện thoại');
      return false;
    }

    if (!patternPhone.test(this.userItem.phoneNumber)) {
      this.msgService.msgBoxError('Số điện thoại không đúng định dạng');
      return false;
    }

    if (!this.userItem.email) {
      this.msgService.msgBoxError('Vui lòng nhập email');
      return false;
    }

    if (!patternEmail.test(this.userItem.email)) {
      this.msgService.msgBoxError('Email không đúng định dạng');
      return false;
    }

    if (!this.selectedManageHub) {
      this.msgService.msgBoxError('Vui lòng chọn đơn vị quản lý');
      return false;
    }

    if (!this.selectedHub) {
      this.msgService.msgBoxError('Vui lòng chọn đơn vị làm việc');
      return false;
    }

    return true;
  }

  isValidData(): boolean {

    const strongPassword = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])");
    const patternPhone = /((03|05|07|08|09)[0-9])+([0-9]{7})\b/;
    const patternPassword = /\s/g;
    if (!this.userItem.code) {
      this.msgService.msgBoxError('Vui lòng nhập mã tài khoản');
      return false;
    }

    if (!this.userItem.userName) {
      this.msgService.msgBoxError('Vui lòng nhập tên');
      return false;
    }

    if (!this.userItem.password) {
      this.msgService.msgBoxError('Vui lòng nhập mật khẩu');
      return false;
    }

    if (patternPassword.test(this.userItem.password)) {
      this.msgService.msgBoxError('Mật khẩu không tồn tại khoản trắng');
      return false;
    }

    if (!strongPassword.test(this.userItem.password)) {
      this.msgService.msgBoxError('Mật khẩu phải có đủ chữ và số');
      return false;
    }

    if (this.userItem.password.length < 8) {
      this.msgService.msgBoxError('Vui lòng nhập mật khẩu từ 8 đến 16 kí tự');
      return false;
    }

    if (!this.userItem.fullName) {
      this.msgService.msgBoxError('Vui lòng nhập đầy đủ họ tên');
      return false;
    }

    if (!this.userItem.phoneNumber) {
      this.msgService.msgBoxError('Vui lòng nhập số điện thoại');
      return false;
    }

    if (!patternPhone.test(this.userItem.phoneNumber)) {
      this.msgService.msgBoxError('Số điện thoại không đúng định dạng');
      return false;
    }

    if (!this.userItem.email) {
      this.msgService.msgBoxError('Vui lòng nhập email');
      return false;
    }

    if (!this.selectedManageHub) {
      this.msgService.msgBoxError('Vui lòng chọn đơn vị quản lý');
      return false;
    }

    if (!this.selectedDepartment) {
      this.msgService.msgBoxError('Vui lòng chọn đơn vị làm việc');
      return false;
    }
    return true;
  }

  initMap() {
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
          this.userItem.address = placeResult.formatted_address;
          // verify result
          if (!placeResult.geometry) {
          }
          let locationInfo = new InfoLocation();
          const geocoder = new google.maps.Geocoder();
          geocoder.geocode({ location: placeResult.geometry.location }, async (results, status) => {
            if (status.toString() === 'OK' && results[0]) {
              locationInfo = await this.hubService.getLocationByGeocoderResult(results);
              if (this.userItem) {
                this.userItem.address = placeResult.formatted_address;
                this.userItem.oldLat = placeResult.geometry.location.lat();
                this.userItem.oldLng = placeResult.geometry.location.lng();
              }
            } else {
              this.msgService.msgBoxError('Không tìm thấy địa chỉ');
            }
          });
        });
      });
    });
  }
}
