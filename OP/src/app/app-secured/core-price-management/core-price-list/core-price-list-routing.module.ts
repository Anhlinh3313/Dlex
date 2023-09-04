import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePriceListComponent } from './core-price-list.component';

const routes: Routes = [
  { path: '', component: CorePriceListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePriceListRoutingModule { }
