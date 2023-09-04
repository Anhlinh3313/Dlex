import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CoreCountryComponent } from './core-country/core-country.component';
import { CorePlaceLocationRoutingModule } from './core-place-location-routing.module';
import { CoreProvinceComponent } from './core-province/core-province.component';
import { CreateUpdateCountryComponent } from './core-dialog/create-update-country/create-update-country.component';
import { CreateUpdateProvinceComponent } from './core-dialog/create-update-province/create-update-province.component';
import { CreateUpdateDistrictComponent } from './core-dialog/create-update-district/create-update-district.component';
import { CoreDistrictComponent } from './core-district/core-district.component';
import { CoreWardComponent } from './core-ward/core-ward.component';
import { CreateUpdateWardComponent } from './core-dialog/create-update-ward/create-update-ward.component';

@NgModule({
  declarations: [
    CoreCountryComponent,
    CoreProvinceComponent,
    CoreDistrictComponent,
    CoreWardComponent,
    CreateUpdateCountryComponent,
    CreateUpdateProvinceComponent,
    CreateUpdateDistrictComponent,
    CreateUpdateWardComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    CalendarModule,
    DialogModule,
    TableModule,
    CheckboxModule,
    DropdownModule,
    RadioButtonModule,
    CorePlaceLocationRoutingModule
  ],
  exports: [
  ],
})
export class CorePlaceLocationModule { }
