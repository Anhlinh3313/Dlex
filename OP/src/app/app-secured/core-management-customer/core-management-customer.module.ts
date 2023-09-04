import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreManagementCustomerRoutingModule } from './core-management-customer-routing.module';
import { CoreCustomerInfoComponent } from './core-customer-info/core-customer-info.component';
import { FormsModule } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { CreateOrUpdateCustomerComponent } from './core-dialog-customer/create-or-update-customer/create-or-update-customer.component';
import { DropdownModule } from 'primeng/dropdown';
import { CheckboxModule } from 'primeng/checkbox';
import { CalendarModule } from 'primeng/calendar';
import { MultiSelectModule } from 'primeng/multiselect';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { GooglePlaceModule } from 'ngx-google-places-autocomplete';
import { CorePriceListSettingComponent } from './core-price-list-setting/core-price-list-setting.component';
import { CoreCustomerInfoLogComponent } from './core-customer-info-log/core-customer-info-log.component';
import { TabViewModule } from 'primeng/tabview';
import { CreateOrUpdateCustomerInfoLogComponent } from './core-dialog-customer/create-or-update-customer-info-log/create-or-update-customer-info-log.component';
import { GeocodingApiService } from 'src/app/shared/services/api/geocodingApiService.service';
import {FileUploadModule} from 'primeng/fileupload';
import {HttpClientModule} from '@angular/common/http';

@NgModule({
  declarations: [
    CoreCustomerInfoComponent,
    CreateOrUpdateCustomerComponent,
    CorePriceListSettingComponent,
    CoreCustomerInfoLogComponent,
    CreateOrUpdateCustomerInfoLogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    DialogModule,
    TableModule,
    DropdownModule,
    CheckboxModule,
    CalendarModule,
    MultiSelectModule,
    AutoCompleteModule,
    GooglePlaceModule,
    CoreManagementCustomerRoutingModule,
    TabViewModule,
    FileUploadModule,
    HttpClientModule
  ],
  providers: [
    GeocodingApiService,
  ],
})
export class CoreManagementCustomerModule { }
