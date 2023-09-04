import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/api/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthLoginGuard implements CanActivate {

    constructor(private authService: AuthService,
                private router: Router) { }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if (this.authService.isLogged()) {
            this.router.navigate(['/']);
            return false;
        }
        else {
            return true;
        }
    }
}
