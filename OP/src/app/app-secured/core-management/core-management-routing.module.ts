import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreManagementComponent } from './core-management.component';

const routes: Routes = [
  { path: '', component: CoreManagementComponent, children: [
    { path: 'service', loadChildren: () => import('./core-service/core-service.module').then(m => m.CoreServiceModule) },
    { path: 'payments', loadChildren: () => import('./core-payments/core-payments.module').then(m => m.CorePaymentsModule) },
    { path: 'type-cargo', loadChildren: () => import('./core-type-cargo/core-type-cargo.module').then(m => m.CoreTypeCargoModule) },
    { path: 'type-shipping', loadChildren: () => import('./core-type-shipping/core-type-shipping.module').then(m => m.CoreTypeShippingModule) },
    { path: 'print-form-management', loadChildren: () => import('./core-print-form-management/core-print-form-management.module').then(m => m.CorePrintFormManagementModule) },
    { path: 'holiday-management', loadChildren: () => import('./core-holiday-management/core-holiday-management.module').then(m => m.CoreHolidayManagementModule) },
    { path: 'bank-accounts', loadChildren: () => import('./core-bank-accounts/core-bank-accounts.module').then(m => m.CoreBankAccountsModule) },
    { path: 'reason-management', loadChildren: () => import('./core-reason-management/core-reason-management.module').then(m => m.CoreReasonManagementModule) },
    { path: 'type-of-complain', loadChildren: () => import('./core-type-of-complain/core-type-of-complain.module').then(m => m.CoreTypeOfComplainModule) },
    { path: 'price-formula', loadChildren: () => import('./core-price-formula/core-price-formula.module').then(m => m.CorePriceFormulaModule) },
    { path: 'discount-recipe', loadChildren: () => import('./core-discount-recipe/core-discount-recipe.module').then(m => m.CoreDiscountRecipeModule) },
    { path: 'pricing-type', loadChildren: () => import('./core-pricing-type/core-pricing-type.module').then(m => m.CorePricingTypeModule) },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreManagementRoutingModule { }
