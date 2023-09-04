import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePoHubComponent } from './core-po-hub.component';

const routes: Routes = [
  { path: '', component: CorePoHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePoHubRoutingModule { }
