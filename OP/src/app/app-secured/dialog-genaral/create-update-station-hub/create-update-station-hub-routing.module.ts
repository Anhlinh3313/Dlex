import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateStationHubComponent } from './create-update-station-hub.component';

const routes: Routes = [
  { path: '', component: CreateUpdateStationHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateStationHubRoutingModule { }
