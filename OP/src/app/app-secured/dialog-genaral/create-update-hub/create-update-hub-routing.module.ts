import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateHubComponent } from './create-update-hub.component';

const routes: Routes = [
  { path: '', component: CreateUpdateHubComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateHubRoutingModule { }
