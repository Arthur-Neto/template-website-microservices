import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '@env';

import { LocalStorageService } from '../local-storage/local-storage.service';
import {
    IAuthenticateCommand,
    IAuthenticatedUser,
} from './authentication-models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    public user: Observable<IAuthenticatedUser | null>;

    public get userValue(): IAuthenticatedUser | null {
        return this.userSubject.value;
    }

    private userSubject: BehaviorSubject<IAuthenticatedUser | null>;

    constructor(
        private localStorageService: LocalStorageService,
        private router: Router,
        private http: HttpClient
    ) {
        this.userSubject = new BehaviorSubject<IAuthenticatedUser | null>(
            this.getToken()
        );
        this.user = this.userSubject.asObservable();
    }

    public login(
        command: IAuthenticateCommand
    ): Observable<IAuthenticatedUser> {
        return this.http
            .post<IAuthenticatedUser>(
                `${environment.apiUrl}api/users/login`,
                command
            )
            .pipe(
                map((user: IAuthenticatedUser) => {
                    this.localStorageService.setItem(
                        'user',
                        JSON.stringify(user)
                    );
                    this.userSubject.next(user);

                    return user;
                })
            );
    }

    public logout(): void {
        this.localStorageService.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['auth/login']);
    }

    private getToken(): IAuthenticatedUser {
        const token = this.localStorageService.getItem('user');

        return token ? JSON.parse(token) : null;
    }
}
