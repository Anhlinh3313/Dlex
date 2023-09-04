import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreRouteHubComponent } from './core-route-hub.component';

const routes: Routes = [
  { path: '', component: CoreRouteHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRouteHubRoutingModule { }
