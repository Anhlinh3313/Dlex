import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreDiscountRecipeComponent } from './core-discount-recipe.component';

const routes: Routes = [
  { path: '', component: CoreDiscountRecipeComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreDiscountRecipeRoutingModule { }
