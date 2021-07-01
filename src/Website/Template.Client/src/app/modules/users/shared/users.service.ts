import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@env';

import {
    IUserCreateCommand,
    IUsersGridModel,
    IUserUpdateCommand,
} from './users.model';

@Injectable()
export class UsersApiService {
    private apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${environment.apiUrl}api/users`;
    }

    public create(command: IUserCreateCommand): Observable<number> {
        return this.http.post<number>(`${this.apiUrl}`, command);
    }

    public update(command: IUserUpdateCommand): Observable<boolean> {
        return this.http.put<boolean>(`${this.apiUrl}`, command);
    }
}

@Injectable()
export class UsersODataService {
    private odataUrl: string;

    constructor(private http: HttpClient) {
        this.odataUrl = `${environment.apiUrl}odata/users`;
    }

    public get(): Observable<IUsersGridModel[]> {
        return this.http.get<IUsersGridModel[]>(`${this.odataUrl}`);
    }
}
