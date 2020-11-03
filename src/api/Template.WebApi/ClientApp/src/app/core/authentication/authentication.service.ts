import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@env';

import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { LocalStorageService } from '../local-storage/local-storage.service';
import { AuthenticateCommand, AuthenticatedUser } from './authentication-models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    public user: Observable<AuthenticatedUser>;

    public get userValue(): AuthenticatedUser {
        return this.userSubject.value;
    }

    private userSubject: BehaviorSubject<AuthenticatedUser>;

    constructor(
        private localStorageService: LocalStorageService,
        private router: Router,
        private http: HttpClient
    ) {
        this.userSubject = new BehaviorSubject<AuthenticatedUser>(this.getToken());
        this.user = this.userSubject.asObservable();
    }

    public login(command: AuthenticateCommand): Observable<AuthenticatedUser> {
        return this.http.post<AuthenticatedUser>(`${ environment.apiUrl }api/users/login`, command)
            .pipe(map((user: AuthenticatedUser) => {
                this.localStorageService.setItem('user', JSON.stringify(user));
                this.userSubject.next(user);

                return user;
            }));
    }

    public logout(): void {
        this.localStorageService.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['auth/login']);
    }

    private getToken(): AuthenticatedUser {
        return this.localStorageService.getItem('user') ? JSON.parse(this.localStorageService.getItem('user')) : null;
    }
}
