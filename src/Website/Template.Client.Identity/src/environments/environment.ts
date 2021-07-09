// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
import { AuthConfig } from 'angular-oauth2-oidc';

export const environment = {
    production: false,
    apiUrl: 'http://localhost:8001/',
};

export const authConfig: AuthConfig = {
    issuer: 'https://localhost:44313/',
    clientId: 'template',
    postLogoutRedirectUri: 'http://localhost:4200/',
    redirectUri: 'http://localhost:4200/',
    userinfoEndpoint: 'https://localhost:44313/connect/userinfo',
    tokenEndpoint: 'https://localhost:44313/connect/token',
    scope: 'email profile roles',
    oidc: false,
    responseType: 'code',
    dummyClientSecret: '901564A5-E7FE-42CB-B10D-61EF6A8F3654',
    showDebugInformation: true,
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
