import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreGeneralManagementComponent } from './core-general-management.component';

const routes: Routes = [
  { path: '', component: CoreGeneralManagementComponent, children: [
    { path: 'users', loadChildren: () => import('./core-users/core-users.module').then(m => m.CoreUsersModule) },
    { path: 'roles', loadChildren: () => import('./core-roles/core-roles.module').then(m => m.CoreRolesModule) },
    { path: 'role-pages', loadChildren: () => import('./core-role-pages/core-role-pages.module').then(m => m.CoreRolePagesModule) },
    { path: 'core-user-relation', loadChildren: () => import('./core-user-relation/core-user-relation.module').then(m => m.CoreUserRelationModule) },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreGeneralManagementRoutingModule { }
