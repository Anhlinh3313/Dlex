import { Component, OnInit, Input } from '@angular/core';
import { AppSecuredComponent } from 'src/app/app-secured/app-secured.component';
import { ChangePasswordComponent } from '../../../app-secured/dialog-genaral/change-password/change-password.component';
import { environment } from 'src/environments/environment';
import { SelectModel } from '../../models/entity/Selected.model';
import { AuthService } from '../../services/api/auth.service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PersonalInformationComponent } from 'src/app/app-secured/dialog-genaral/personal-information/personal-information.component';
import { Constant } from '../../infrastructure/constant';
import { HubService } from '../../services/api/hub.service';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styles: [
  ]

})

export class TopbarComponent implements OnInit {

  envir = environment;
  activeItem: number;
  // Data
  hubs: SelectModel[] = [];
  //
  selectedHubs: any;
  searchText = '';
  ref: DynamicDialogRef;
  fullName: string;
  firstName: string;
  lastName: string;
  avatar: string;
  hubName: any;

  constructor(
    public app: AppSecuredComponent,
    private authService: AuthService,
    protected dialogService: DialogService,
    protected hubService: HubService,

  ) { }

  async ngOnInit(): Promise<void> {
    await this.loadUser();
    await this.loadHubName();
    // this.user.fullName = this.authService.getFullName();
  }

  async loadHubName() {
    let res = await this.hubService.getHubByUserId(localStorage.getItem(Constant.auths.userId))
    this.hubName = res[0].hubName;
  }

  async loadUser() {
    const result = await this.authService.getUserById(localStorage.getItem(Constant.auths.userId));
    this.fullName = result.data[0].fullName;
    this.firstName = this.fullName.split(' ').slice(0, 1).join(' ');
    this.lastName = this.fullName.split(' ').slice(1).join(' ');
    this.avatar = this.firstName.charAt(0).toUpperCase() + this.lastName.charAt(0).toUpperCase();
  }

  mobileMegaMenuItemClick(index): void {
    this.app.megaMenuMobileClick = true;
    this.activeItem = this.activeItem === index ? null : index;
  }

  changePassword(item: any = null): void {
    this.ref = this.dialogService.open(ChangePasswordComponent, {
      header: `${'Đổi mật khẩu'}`,
      width: '25%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: item,
    });
    this.ref.onClose.subscribe((res: any) => {
    });
  }

  personalInformation(item: any = null): void {
    this.ref = this.dialogService.open(PersonalInformationComponent, {
      header: `${'Thông tin cá nhân'}`,
      width: '50%',
      contentStyle: { 'max-height': '500px', overflow: 'inherit' },
      styleClass: 'dialog-user',
      baseZIndex: 10000,
      data: item,
    });
    this.ref.onClose.subscribe((res: any) => {
    });
  }

  logout(): void {
    this.authService.logout();
  }


}
