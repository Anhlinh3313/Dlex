import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreManagementCustomerComponent } from './core-management-customer.component';

const routes: Routes = [
  {
    path: '', component: CoreManagementCustomerComponent, children: [
      { path: 'customer-info', loadChildren: () => import('./core-customer-info/core-customer-info.module').then(m => m.CoreCustomerInfoModule) },
      { path: 'price-list-setting', loadChildren: () => import('./core-price-list-setting/core-price-list-setting.module').then(m => m.CorePriceListSettingModule) },
      { path: 'customer-info-log', loadChildren: () => import('./core-customer-info-log/core-customer-info-log.module').then(m => m.CoreCustomerInfoLogModule) },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreManagementCustomerRoutingModule { }
