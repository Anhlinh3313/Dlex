import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdatePoHubComponent } from './create-update-po-hub.component';

const routes: Routes = [
  { path: '', component: CreateUpdatePoHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdatePoHubRoutingModule { }
