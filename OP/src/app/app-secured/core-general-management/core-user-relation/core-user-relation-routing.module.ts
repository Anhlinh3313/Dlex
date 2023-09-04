import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreUserRelationComponent } from './core-user-relation.component';

const routes: Routes = [
  { path: '', component: CoreUserRelationComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreUserRelationRoutingModule { }
