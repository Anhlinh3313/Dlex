import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePricingTypeComponent } from './core-pricing-type.component';

const routes: Routes = [
  { path: '', component: CorePricingTypeComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePricingTypeRoutingModule { }
