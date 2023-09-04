import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreBankAccountsComponent } from './core-bank-accounts.component';

const routes: Routes = [
  { path: '', component: CoreBankAccountsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreBankAccountsRoutingModule { }
