import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateUserComponent } from './create-update-user.component';

const routes: Routes = [
  { path: '', component: CreateUpdateUserComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateUserRoutingModule { }
