import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRouteHubRoutingModule } from './core-route-hub-routing.module';
import { TooltipModule } from 'primeng/tooltip';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CoreRouteHubRoutingModule,
    TooltipModule,
  ]
})
export class CoreRouteHubModule { }
