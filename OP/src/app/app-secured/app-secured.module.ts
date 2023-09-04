import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppSecuredRoutingModule } from './app-secured-routing.module';
import { CoreGeneralManagementComponent } from './core-general-management/core-general-management.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { MultiSelectModule } from 'primeng/multiselect';
import { ChangePasswordComponent } from './dialog-genaral/change-password/change-password.component';
import { CorePlaceLocationComponent } from './core-place-location/core-place-location.component';
import { CoreHubManagementComponent } from './core-hub-management/core-hub-management.component';
import { CreateUpdateHubComponent } from './dialog-genaral/create-update-hub/create-update-hub.component';
import { DropdownModule } from 'primeng/dropdown';
import { CreateUpdatePoHubComponent } from './dialog-genaral/create-update-po-hub/create-update-po-hub.component';
import { CreateUpdateStationHubComponent } from './dialog-genaral/create-update-station-hub/create-update-station-hub.component';
import { CreateUpdateRoutingHubComponent } from './dialog-genaral/create-update-routing-hub/create-update-routing-hub.component';
import { PersonalInformationComponent } from './dialog-genaral/personal-information/personal-information.component';
import { TooltipModule } from 'primeng/tooltip';
import { CoreManagementComponent } from './core-management/core-management.component';
import { CorePriceManagementComponent } from './core-price-management/core-price-management.component';
import { CoreManagementCustomerComponent } from './core-management-customer/core-management-customer.component';
import { GooglePlaceModule } from 'ngx-google-places-autocomplete';

@NgModule({
  declarations: [
    CoreGeneralManagementComponent,
    CorePlaceLocationComponent,
    WelcomeComponent,
    ChangePasswordComponent,
    CoreHubManagementComponent,
    CreateUpdateHubComponent,
    CreateUpdatePoHubComponent,
    CreateUpdateStationHubComponent,
    CreateUpdateRoutingHubComponent,
    PersonalInformationComponent,
    CoreManagementComponent,
    CorePriceManagementComponent,
    CoreManagementCustomerComponent,
  ],
  imports: [
    FormsModule,
    SharedModule,
    CommonModule,
    CalendarModule,
    DialogModule,
    DropdownModule,
    CheckboxModule,
    RadioButtonModule,
    TableModule,
    MultiSelectModule,
    AppSecuredRoutingModule,
    TooltipModule,
  ],
  entryComponents: [
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
  ]
})
export class AppSecuredModule { }
