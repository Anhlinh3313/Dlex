import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateRoleComponent } from './create-update-role.component';

const routes: Routes = [
  { path: '', component: CreateUpdateRoleComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateRoleRoutingModule { }
