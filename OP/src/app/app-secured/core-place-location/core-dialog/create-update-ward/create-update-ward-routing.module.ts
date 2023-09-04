import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateWardComponent } from './create-update-ward.component';

const routes: Routes = [
  { path: '', component: CreateUpdateWardComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateWardRoutingModule { }
