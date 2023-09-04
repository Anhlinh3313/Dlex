import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePriceListSettingComponent } from './core-price-list-setting.component';

const routes: Routes = [
  { path: '', component: CorePriceListSettingComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePriceListSettingRoutingModule { }
