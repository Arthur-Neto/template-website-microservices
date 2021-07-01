import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    Router,
    RouterStateSnapshot,
} from '@angular/router';

import { AuthenticationService } from '../authentication/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {}

    public canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        const user = this.authenticationService.userValue;
        if (user) {
            if (
                route.data.roles &&
                route.data.roles.indexOf(user.role) === -1
            ) {
                this.router.navigate(['dashboard']);
                return false;
            }

            return true;
        }

        this.router.navigate(['login'], {
            queryParams: { returnUrl: state.url },
        });
        return false;
    }
}
