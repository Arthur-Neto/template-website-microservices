import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env';

import { Observable } from 'rxjs';

import { IUserCreateCommand, IUserUpdateCommand } from './users.model';

@Injectable()
export class UsersApiService {
    private apiUrl: string;

    constructor(
        private http: HttpClient
    ) {
        this.apiUrl = `${ environment.apiUrl }api/users`;
    }

    public create(command: IUserCreateCommand): Observable<number> {
        return this.http.post<number>(`${ this.apiUrl }`, command);
    }

    public update(command: IUserUpdateCommand): Observable<boolean> {
        return this.http.put<boolean>(`${ this.apiUrl }`, command);
    }
}
