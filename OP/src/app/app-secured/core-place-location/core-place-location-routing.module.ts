import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePlaceLocationComponent } from './core-place-location.component';

const routes: Routes = [
  { path: '', component: CorePlaceLocationComponent, children: [
    { path: 'country', loadChildren: () => import('./core-country/core-country.module').then(m => m.CoreCountryModule) },
    { path: 'province', loadChildren: () => import('./core-province/core-province.module').then(m => m.CoreProvinceModule) },
    { path: 'district', loadChildren: () => import('./core-district/core-district.module').then(m => m.CoreDistrictModule) },
    { path: 'ward', loadChildren: () => import('./core-ward/core-ward.module').then(m => m.CoreWardModule) },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePlaceLocationRoutingModule { }
