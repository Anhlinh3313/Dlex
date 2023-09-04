import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateCountryComponent } from './create-update-country.component';

const routes: Routes = [
  { path: '', component: CreateUpdateCountryComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateCountryRoutingModule { }
