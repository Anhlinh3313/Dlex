import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreServiceComponent } from './core-service.component';

const routes: Routes = [
  { path: '', component: CoreServiceComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreServiceRoutingModule { }
