import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateOrUpdateCustomerComponent } from './create-or-update-customer.component';

const routes: Routes = [
  { path: '', component: CreateOrUpdateCustomerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateOrUpdateCustomerRoutingModule { }
