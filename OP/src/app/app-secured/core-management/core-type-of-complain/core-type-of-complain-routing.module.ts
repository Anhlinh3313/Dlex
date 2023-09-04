import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreTypeOfComplainComponent } from './core-type-of-complain.component';

const routes: Routes = [
  { path: '', component: CoreTypeOfComplainComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreTypeOfComplainRoutingModule { }
