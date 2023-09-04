import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CorePriceManagementRoutingModule } from './core-price-management-routing.module';
import { CoreDeductPriceComponent } from './core-deduct-price/core-deduct-price.component';
import { CoreDeductPriceCustomerComponent } from './core-deduct-price-customer/core-deduct-price-customer.component';
import { FormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { MultiSelectModule } from 'primeng/multiselect';
import { DateFormatPipe } from 'src/app/shared/pipes/dateFormat.pipe';
import { CoreApplyingPriceCustomerComponent } from './core-applying-price-customer/core-applying-price-customer.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CorePriceListComponent } from './core-price-list/core-price-list.component';
import { TabViewModule } from 'primeng/tabview';
import { GooglePlaceModule } from 'ngx-google-places-autocomplete';
import {InputSwitchModule} from 'primeng/inputswitch';
import { CoreCopyPriceComponent } from './core-dialog-price-management/core-copy-price/core-copy-price.component';

@NgModule({
  declarations: [
    CoreDeductPriceComponent,
    CoreDeductPriceCustomerComponent,
    CoreApplyingPriceCustomerComponent,
    DateFormatPipe,
    CorePriceListComponent,
    CoreCopyPriceComponent,
  ],
  imports: [
    CommonModule,
    CorePriceManagementRoutingModule,
    FormsModule,
    CalendarModule,
    DialogModule,
    TableModule,
    CheckboxModule,
    DropdownModule,
    RadioButtonModule,
    MultiSelectModule,
    AutoCompleteModule,
    TabViewModule,
    GooglePlaceModule,
    InputSwitchModule,
  ]
})
export class CorePriceManagementModule { }
