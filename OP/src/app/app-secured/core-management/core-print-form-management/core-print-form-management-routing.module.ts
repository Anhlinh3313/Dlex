import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePrintFormManagementComponent } from './core-print-form-management.component';

const routes: Routes = [
  { path: '', component: CorePrintFormManagementComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePrintFormManagementRoutingModule { }
