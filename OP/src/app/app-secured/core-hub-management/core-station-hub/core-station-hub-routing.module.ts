import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreStationHubComponent } from './core-station-hub.component';

const routes: Routes = [
  { path: '', component: CoreStationHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreStationHubRoutingModule { }
