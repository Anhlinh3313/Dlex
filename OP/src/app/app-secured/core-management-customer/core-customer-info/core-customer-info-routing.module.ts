import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreCustomerInfoComponent } from './core-customer-info.component';

const routes: Routes = [
  { path: '', component: CoreCustomerInfoComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreCustomerInfoRoutingModule { }
