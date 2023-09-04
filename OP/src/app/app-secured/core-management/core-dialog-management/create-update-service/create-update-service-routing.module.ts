import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateServiceComponent } from './create-update-service.component';

const routes: Routes = [
  { path: '', component: CreateUpdateServiceComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateServiceRoutingModule { }
