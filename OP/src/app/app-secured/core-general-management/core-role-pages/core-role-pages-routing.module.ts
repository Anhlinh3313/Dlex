import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreRolePagesComponent } from './core-role-pages.component';

const routes: Routes = [
  { path: '', component: CoreRolePagesComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRolePagesRoutingModule { }
