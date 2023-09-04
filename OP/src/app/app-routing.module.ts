import { NgModule } from '@angular/core';
import { Routes, RouterModule, NoPreloading } from '@angular/router';
import { Page404Component } from './shared/components/403/403.component';
import { AuthGuard } from './shared/guard/auth.guard';
import { AuthLoginGuard } from './shared/guard/authLogin.guard';

const routes: Routes = [
  { path: 'login',
    loadChildren: () => import('./login/login.module').then(m => m.LoginModule),
    canActivate: [AuthLoginGuard]
  },
  { path: '',
    loadChildren: () => import('./app-secured/app-secured.module').then(m => m.AppSecuredModule),
    // canActivate: [AuthGuard], canActivateChild: [AuthGuard]
  },
  { path: '403', component: Page404Component },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top', preloadingStrategy: NoPreloading })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
