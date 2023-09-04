import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUpdateProvinceComponent } from './create-update-province.component';

const routes: Routes = [
  { path: '', component: CreateUpdateProvinceComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateUpdateProvinceRoutingModule { }
