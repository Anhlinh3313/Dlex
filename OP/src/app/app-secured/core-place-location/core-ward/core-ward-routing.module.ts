import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreWardComponent } from './core-ward.component';

const routes: Routes = [
  { path: '', component: CoreWardComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreWardRoutingModule { }
