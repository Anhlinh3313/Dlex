import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppSecuredComponent } from './app-secured.component';

const routes: Routes = [
  { path: '', component: AppSecuredComponent, children: [
    { path: '',
      loadChildren: () => import('./welcome/welcome.module').then(m => m.WelcomeModule)
    },
    { path: 'core-general',
      loadChildren: () => import('./core-general-management/core-general-management.module').then(m => m.CoreGeneralManagementModule)
    },
    { path: 'core-place',
      loadChildren: () => import('./core-place-location/core-place-location.module').then(m => m.CorePlaceLocationModule)
    },
    { path: 'core-hub',
      loadChildren: () => import('./core-hub-management/core-hub-management.module').then(m => m.CoreHubManagementModule)
    },
    { path: 'core-management',
      loadChildren: () => import('./core-management/core-management.module').then(m => m.CoreManagementModule)
    },
    { path: 'core-price-management',
    loadChildren: () => import('./core-price-management/core-price-management.module').then(m => m.CorePriceManagementModule)
  },
    { path: 'core-management-customer',
      loadChildren: () => import('./core-management-customer/core-management-customer.module').then(m => m.CoreManagementCustomerModule)
    },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppSecuredRoutingModule { }
