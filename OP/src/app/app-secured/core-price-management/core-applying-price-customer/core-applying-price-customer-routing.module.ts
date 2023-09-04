import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreApplyingPriceCustomerComponent } from './core-applying-price-customer.component';

const routes: Routes = [
  { path: '', component: CoreApplyingPriceCustomerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreApplyingPriceCustomerRoutingModule { }
