import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class IsAuthenticatedUserManagerGuard implements CanActivate {
    // constructor(private authenticationService: AuthenticationService) {}

    public canActivate(): boolean {
        /*if (this.authenticationService.userValue?.role === Role.Manager) {
            return true;
        }

        this.authenticationService.logout();
        return false;*/

        return true;
    }
}
