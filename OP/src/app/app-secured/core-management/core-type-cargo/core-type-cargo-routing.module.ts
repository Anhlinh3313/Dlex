import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreTypeCargoComponent } from './core-type-cargo.component';

const routes: Routes = [
  { path: '', component: CoreTypeCargoComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreTypeCargoRoutingModule { }
