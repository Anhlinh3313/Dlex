import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateDistrictComponent } from './create-update-district.component';

const routes: Routes = [
  { path: '', component: CreateUpdateDistrictComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateDistrictRoutingModule { }
