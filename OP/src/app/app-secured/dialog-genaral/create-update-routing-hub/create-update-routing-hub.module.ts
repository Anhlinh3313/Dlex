import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateUpdateRoutingHubRoutingModule } from './create-update-routing-hub-routing.module';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TableModule,
    CheckboxModule,
    CreateUpdateRoutingHubRoutingModule
  ]
})
export class CreateUpdateRoutingHubModule { }
