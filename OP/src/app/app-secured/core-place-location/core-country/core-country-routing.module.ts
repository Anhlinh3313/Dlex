import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreCountryComponent } from './core-country.component';

const routes: Routes = [
  { path: '', component: CoreCountryComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreCountryRoutingModule { }
