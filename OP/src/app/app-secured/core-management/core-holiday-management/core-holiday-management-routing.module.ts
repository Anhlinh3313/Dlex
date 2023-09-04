import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreHolidayManagementComponent } from './core-holiday-management.component';

const routes: Routes = [
  { path: '', component: CoreHolidayManagementComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreHolidayManagementRoutingModule { }
