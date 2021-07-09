import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, Subject } from 'rxjs';

import { IAuthenticatedUser } from '@core/authentication/authentication-models';

@Component({
    selector: 'app-nav-bar-login',
    templateUrl: './nav-bar-login.component.html',
    styleUrls: ['./nav-bar-login.component.scss'],
})
export class NavBarLoginComponent implements OnDestroy {
    public userLogged$: Observable<IAuthenticatedUser>;

    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(
        private router: Router,
        private oauthService: OAuthService,
        private store: Store<{ userLogged: IAuthenticatedUser }>
    ) {
        this.userLogged$ = this.store.pipe(select('userLogged'));
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public login(): void {
        this.oauthService.initLoginFlow();
    }

    public logout(): void {
        this.oauthService.logOut();
    }

    public editLogin(): void {
        // this.router.navigate(['auth/edit']);
    }
}
