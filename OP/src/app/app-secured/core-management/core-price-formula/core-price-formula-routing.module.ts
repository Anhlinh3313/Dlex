import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CorePriceFormulaComponent } from './core-price-formula.component';

const routes: Routes = [
  { path: '', component: CorePriceFormulaComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorePriceFormulaRoutingModule { }
