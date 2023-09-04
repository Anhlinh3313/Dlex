import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreTypeShippingComponent } from './core-type-shipping.component';

const routes: Routes = [
  { path: '', component: CoreTypeShippingComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreTypeShippingRoutingModule { }
