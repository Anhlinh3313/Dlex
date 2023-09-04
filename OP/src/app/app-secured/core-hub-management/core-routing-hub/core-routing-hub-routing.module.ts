import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreRoutingHubComponent } from './core-routing-hub.component';

const routes: Routes = [
  { path: '', component: CoreRoutingHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingHubRoutingModule { }
