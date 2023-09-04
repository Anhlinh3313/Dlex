import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreRolesComponent } from './core-roles.component';

const routes: Routes = [
  { path: '', component: CoreRolesComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRolesRoutingModule { }
