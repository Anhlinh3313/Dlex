import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreDistrictComponent } from './core-district.component';

const routes: Routes = [
  { path: '', component: CoreDistrictComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreDistrictRoutingModule { }
