import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreDeductPriceCustomerComponent } from './core-deduct-price-customer.component';

const routes: Routes = [
  { path: '', component: CoreDeductPriceCustomerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreDeductPriceCustomerRoutingModule { }
