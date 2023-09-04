import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreCustomerInfoLogComponent } from './core-customer-info-log.component';

const routes: Routes = [
  { path: '', component: CoreCustomerInfoLogComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreCustomerInfoLogRoutingModule { }
