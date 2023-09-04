import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePriceManagementComponent } from './core-price-management.component';

const routes: Routes = [
  { path: '', component: CorePriceManagementComponent, children: [
    { path: 'deduct-price', loadChildren: () => import('./core-deduct-price/core-deduct-price.module').then(m => m.CoreDeductPriceModule) },
    { path: 'deduct-price-customer',
      loadChildren: () => import('./core-deduct-price-customer/core-deduct-price-customer.module').then(
        m => m.CoreDeductPriceCustomerModule
      )
    },
    { path: 'applying-price-customer',
      loadChildren: () => import('./core-applying-price-customer/core-applying-price-customer.module').then(
        m => m.CoreApplyingPriceCustomerModule
      )
    },
    { path: 'price-list', loadChildren: () => import('./core-price-list/core-price-list.module').then(m => m.CorePriceListModule) },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePriceManagementRoutingModule { }
