import { MapsAPILoader } from '@agm/core';
import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BaseComponent } from 'src/app/shared/components/baseComponent';
import { Constant } from 'src/app/shared/infrastructure/constant';
import { SortUtil } from 'src/app/shared/infrastructure/sort.util';
import { IconInputModel } from 'src/app/shared/models/entity/icon.model';
import { InfoLocation } from 'src/app/shared/models/entity/infoLocation.model';
import { SelectModel } from 'src/app/shared/models/entity/Selected.model';
import { User } from 'src/app/shared/models/entity/user.model';
import { UserRole } from 'src/app/shared/models/entity/userRole.model';
import { FilterViewModel } from 'src/app/shared/models/viewModel/filter.viewModel';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { DepartmentService } from 'src/app/shared/services/api/department.service';
import { HubService } from 'src/app/shared/services/api/hub.service';
import { PermissionService } from 'src/app/shared/services/api/permission.service';
import { RoleService } from 'src/app/shared/services/api/role.service';
import { MsgService } from 'src/app/shared/services/local/msg.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-update-user',
  templateUrl: './create-update-user.component.html',
  styleUrls: ['./create-update-user.component.scss']
})
export class CreateUpdateUserComponent extends BaseComponent implements OnInit {
  userItem: User = new User();
  //
  managementHubs: SelectModel[] = [];
  hubs: SelectModel[] = [];
  roles: SelectModel[] = [];
  selectedManageHubId: SelectModel;
  selectedRole: SelectModel[] = [];
  selectedHubId: SelectModel;
  manageHub: string = "1";
  inputElementNewPassword: IconInputModel = new IconInputModel();
  eyeShow = environment.eyeShow;
  eyeHide = environment.eyeHide;
  edit = false;
  listHub: any = [];
  centerHubs: any = [];
  poHubs: any = [];
  stationHubs: any = [];
  roleId: number[];
  userRoles: UserRole[] = [];
  selectedItemsLabel: string;
  hubId: number;
  checkUserName: boolean = false;
  styleColor: string;
  filter: FilterViewModel;
  checkRole: boolean = true;
  //
  @ViewChild('Address') shippingAddressElementRef: ElementRef;
  //
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
  ) {
    super(msgService, router, permissionService);
  }

  ngOnInit(): void {
    this.intData();
  }

  async intData() {
    await this.loadAllHub();
    await this.loadRole();
    this.selectedItemsLabel = `Đã chọn ${0} chức vụ`;
    let userLoginId = parseInt(localStorage.getItem(Constant.auths.userId));
    const result = await this.authService.getInfoUserById(userLoginId);
    const checkRoleId = result.data.roles.find(f => f.id === 1);
    if (this.config.data) {
      if (userLoginId === 1 || checkRoleId) {
        this.checkRole = false;
      }
      this.checkUserName = true;
      this.styleColor = "background-color: #c7c5c2;";
      this.edit = true;
      this.userItem = this.config.data;
      await this.findHubData();
      await this.onChangeManageHubId();
      await this.findHub();
      await this.findRole();
    } else {
      this.checkRole = false;
    }
    this.inputElementNewPassword.src = this.eyeHide;
    this.inputElementNewPassword.type = 'password';
    this.initMap();
  }

  async findHubData() {
    const userHub = this.listHub.find(f => f.id === this.userItem.manageHubId);
    if (userHub) {
      if (!userHub.centerHubId) {
        this.manageHub = "1";
      } else if (userHub.centerHubId && !userHub.poHubId) {
        this.manageHub = "2";
      } else if (userHub.centerHubId && userHub.poHubId) {
        this.manageHub = "3";
      }
      await this.loadAllHub();
      const findManageHub = this.managementHubs.find(f => f.value === userHub.id);
      this.selectedManageHubId = findManageHub;
    }
  }

  findHub() {
    const findHubId = this.hubs.find(f => f.value === this.userItem.hubId);
    if (findHubId) {
      this.selectedHubId = findHubId;
    }
  }

  findRole() {
    this.selectedRole = [];
    this.userItem.roles.forEach(fe => {
      const findRole = this.roles.find(f => f.value === fe.id);
      if (findRole) {
        this.selectedRole.push(findRole);
      }
    });
    this.selectedItemsLabel = `Đã chọn ${this.selectedRole.length} chức vụ`;
  }

  onChangeUserRoles(): void {
    this.userItem.roleIds = [];
    this.selectedRole.map(m => {
      if (m.value) {
        this.userItem.roleIds.push(m.value);
      }
    });
    this.selectedItemsLabel = `Đã chọn ${this.selectedRole.length} chức vụ`;
  }

  async onChangeManageHubId() {
    this.userItem.manageHubId = this.selectedManageHubId.value;
    this.hubId = this.selectedManageHubId.value;
    await this.loadHub();
  }

  async loadHub() {
    this.hubs = await this.hubService.getListHubFromHubIdUserSelectModelAsync(this.hubId);
  }

  onChangehub() {
    this.userItem.hubId = this.selectedHubId.value;
  }

  onClick(): void {
    this.msgService.success('Cập nhật của bạn đã được thay đổi trên hệ thống');
  }

  onClickCancel(evet): void {
    if (this.ref) {
      this.ref.close(evet);
    }
  }

  async getDropdownHub(arr, type): Promise<SelectModel[]> {
    let selectModel = [];
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
      data = SortUtil.sortAlphanumerical(data, 1, "code");
      data.forEach(element => {
        selectModel.push({
          label: `${element.code} - ${element.name}`,
          data: element,
          value: element.id
        });
      });
      selectModel.unshift({ label: "-- Chọn đơn vị quản lý --", data: null, value: null });
      return selectModel;
    }
  }

  async loadAllHub() {
    let listHub = await this.hubService.getAll() as any;
    if (listHub.data && listHub.data.length > 0) {
      this.listHub = listHub.data;
      this.centerHubs = await this.getDropdownHub(this.listHub, 1);
      this.poHubs = await this.getDropdownHub(this.listHub, 2);
      this.stationHubs = await this.getDropdownHub(this.listHub, 3);
    }
    this.loadHubCenter();
  }

  LoadCheckRoleEdit() {
    if (!this.checkRole) {
      this.loadHubCenter();
    }
  }

  async loadHubCenter() {
    this.selectedManageHubId = null;
    if (this.manageHub == "1") {
      this.managementHubs = this.centerHubs;
      this.hubs = null;
    }
    if (this.manageHub == "2") {
      this.managementHubs = this.poHubs;
      this.hubs = null;
    }
    if (this.manageHub == "3") {
      this.managementHubs = this.stationHubs;
      this.hubs = null;
    }
  }

  async loadRole() {
    this.roles = await this.roleService.getRoleAsync();
  }

  onClickSave() {
    if (this.edit) {
      this.updateUser();
    } else {
      this.checkCoincideCode();
    }
  }

  async updateUser() {
    if (!this.isValidDataUpdate()) { return; }
    this.userItem.isEnabled = true;
    let res = await this.authService.updateUser(this.userItem)
    if (res.isSuccess) {
      this.msgService.success("Cập nhật của bạn đã được thay đổi trên hệ thống");
      this.onClickCancel(true);
      let userLoginId = localStorage.getItem(Constant.auths.userId)
      if (this.userItem.id == parseFloat(userLoginId)) {
        setTimeout(() => {
          location.reload();
        }, 500);
      }
    } else {
      this.msgService.error(res.message);
    }
  }

  async creatUser() {
    this.userItem.roleIds = []
    this.selectedRole.map(x => {
      if (x.value) {
        this.userItem.roleIds.push(x.value);
      }
    })
    this.userItem.isEnabled = true;
    this.userItem.typeUserId = 2;
    let res = await this.authService.create(this.userItem)
    if (res.isSuccess) {
      this.msgService.success("Tạo mới tài khoản thành công");
      this.onClickCancel(true);
    } else {
      this.msgService.error(res.message);
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

  isValidDataUpdate(): boolean {
    const patternPhone = /((03|05|07|08|09)[0-9])+([0-9]{7})\b/;
    if (!this.userItem.code) {
      this.msgService.error('Vui lòng nhập mã tài khoản');
      return false;
    }

    if (!this.userItem.userName) {
      this.msgService.error('Vui lòng nhập tên');
      return false;
    }

    if (!this.userItem.fullName) {
      this.msgService.error('Vui lòng nhập đầy đủ họ tên');
      return false;
    }

    if (!this.userItem.phoneNumber) {
      this.msgService.error('Vui lòng nhập số điện thoại');
      return false;
    }

    if (!patternPhone.test(this.userItem.phoneNumber)) {
      this.msgService.error('Số điện thoại không đúng định dạng');
      return false;
    }

    if (!this.userItem.email) {
      this.msgService.error('Vui lòng nhập email');
      return false;
    }

    if (!this.selectedManageHubId) {
      this.msgService.error('Vui lòng chọn đơn vị quản lý');
      return false;
    }

    if (!this.selectedHubId) {
      this.msgService.error('Vui lòng chọn đơn vị làm việc');
      return false;
    }

    return true;
  }

  isValidData(): boolean {
    const patternPhone = /((03|05|07|08|09)[0-9])+([0-9]{7})\b/;
    const strongPassword = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])");
    if (!this.userItem.code) {
      this.msgService.error('Vui lòng nhập mã tài khoản');
      return false;
    }

    if (!this.userItem.userName) {
      this.msgService.error('Vui lòng nhập tên');
      return false;
    }

    if (!this.userItem.password) {
      this.msgService.error('Vui lòng nhập mật khẩu');
      return false;
    }

    if (!strongPassword.test(this.userItem.password)) {
      this.msgService.error('Mật khẩu phải có đủ chữ và số');
      return false;
    }

    if (this.userItem.password.length < 8) {
      this.msgService.error('Mật khẩu phải nhiều hơn 8 ký tự.');
      return false;
    }

    if (!this.userItem.fullName) {
      this.msgService.error('Vui lòng nhập đầy đủ họ tên');
      return false;
    }

    if (!this.userItem.phoneNumber) {
      this.msgService.error('Vui lòng nhập số điện thoại');
      return false;
    }

    if (!patternPhone.test(this.userItem.phoneNumber)) {
      this.msgService.error('Số điện thoại không đúng định dạng');
      return false;
    }

    if (!this.userItem.email) {
      this.msgService.error('Vui lòng nhập email');
      return false;
    }

    if (!this.selectedManageHubId) {
      this.msgService.error('Vui lòng chọn đơn vị quản lý');
      return false;
    }

    if (!this.selectedHubId) {
      this.msgService.error('Vui lòng chọn đơn vị làm việc');
      return false;
    }
    return true;
  }

  async checkCoincideCode() {
    if (!this.isValidData()) { return; };
    this.filter = {
      companyId: this.userItem.companyId,
      pageNumber: 1,
      pageSize: 20,
      searchText: this.userItem.code
    };
    let checCode = await this.authService.getUsersBySearchCode(this.filter);
    if (checCode.data.length > 0) {
      this.msgService.error('Mã đã tồn tại');
    } else {
      this.creatUser();
    }
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
              this.messageService.error('Không tìm thấy địa chỉ');
            }
          });
        });
      });
    });
  }
}
