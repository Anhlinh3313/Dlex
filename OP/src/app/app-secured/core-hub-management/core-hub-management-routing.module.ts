import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreHubManagementComponent } from './core-hub-management.component';

const routes: Routes = [
  { path: '', component: CoreHubManagementComponent, children: [
    { path: 'hub',loadChildren: () => import('./core-center-hub/core-center-hub.module').then(m => m.CoreCenterHubModule)}, 
    { path: 'po-hub',loadChildren: () => import('./core-po-hub/core-po-hub.module').then(m => m.CorePoHubModule)}, 
    { path: 'station-hub',loadChildren: () => import('./core-station-hub/core-station-hub.module').then(m => m.CoreStationHubModule)},
    { path: 'route-hub',loadChildren: () => import('./core-route-hub/core-route-hub.module').then(m => m.CoreRouteHubModule)},
    { path: 'routing-hub',loadChildren: () => import('./core-routing-hub/core-routing-hub.module').then(m => m.CoreRoutingHubModule)},

  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreHubManagementRoutingModule { }
