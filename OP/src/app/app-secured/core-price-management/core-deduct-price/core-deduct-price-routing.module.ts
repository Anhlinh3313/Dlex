import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreDeductPriceComponent } from './core-deduct-price.component';

const routes: Routes = [
  { path: '', component: CoreDeductPriceComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreDeductPriceRoutingModule { }
