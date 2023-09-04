import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreCenterHubComponent } from './core-center-hub.component';

const routes: Routes = [
  { path: '', component: CoreCenterHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreCenterHubRoutingModule { }
