import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePaymentsComponent } from './core-payments.component';

const routes: Routes = [
  { path: '', component: CorePaymentsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePaymentsRoutingModule { }
