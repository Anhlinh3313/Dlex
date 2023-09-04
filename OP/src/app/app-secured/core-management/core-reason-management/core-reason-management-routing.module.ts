import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreReasonManagementComponent } from './core-reason-management.component';

const routes: Routes = [
  { path: '', component: CoreReasonManagementComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreReasonManagementRoutingModule { }
