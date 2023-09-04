import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreHubManagementRoutingModule } from './core-hub-management-routing.module';
import { CoreCenterHubComponent } from './core-center-hub/core-center-hub.component';
import { FormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CorePoHubComponent } from './core-po-hub/core-po-hub.component';
import { CoreStationHubComponent } from './core-station-hub/core-station-hub.component';
import { CoreRouteHubComponent } from './core-route-hub/core-route-hub.component';
import { CoreRoutingHubComponent } from './core-routing-hub/core-routing-hub.component';
import { TooltipModule } from 'primeng/tooltip';


@NgModule({
  declarations: [CoreCenterHubComponent, CorePoHubComponent, CoreStationHubComponent, CoreRouteHubComponent, CoreRoutingHubComponent],
  imports: [
    FormsModule,
    CommonModule,
    CalendarModule,
    DialogModule,
    TableModule,
    CheckboxModule,
    DropdownModule,
    RadioButtonModule,
    CoreHubManagementRoutingModule,
    TooltipModule,
  ]
})
export class CoreHubManagementModule { }
