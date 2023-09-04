import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateOrUpdateCustomerInfoLogComponent } from './create-or-update-customer-info-log.component';

const routes: Routes = [
  { path: '', component: CreateOrUpdateCustomerInfoLogComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateOrUpdateCustomerInfoLogRoutingModule { }
