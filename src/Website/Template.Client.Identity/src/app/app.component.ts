import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';

import { IAuthenticatedUser } from '@core/authentication/authentication-models';

import { createUserLogged } from '@layout/nav-bar/nav-bar-login/actions/user-logged.actions';

import { authConfig } from '@env';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})
export class AppComponent {
    constructor(
        private oauthService: OAuthService,
        private store: Store<{ userLogged: IAuthenticatedUser }>
    ) {
        this.configureWithNewConfigApi();
    }

    private configureWithNewConfigApi() {
        this.oauthService.configure(authConfig);
        this.oauthService.setStorage(localStorage);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();

        this.oauthService.loadDiscoveryDocumentAndTryLogin().then((_) => {
            if (
                this.oauthService.hasValidAccessToken() ||
                this.oauthService.hasValidIdToken()
            ) {
                this.oauthService.loadUserProfile().then((user) => {
                    const userLogged: IAuthenticatedUser = {
                        username: user['email'],
                        isAuthenticated: true,
                    };

                    this.store.dispatch(createUserLogged({ userLogged }));
                });
            }
        });
    }
}
