import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreUsersComponent } from './core-users.component';

const routes: Routes = [
  { path: '', component: CoreUsersComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreUsersRoutingModule { }
