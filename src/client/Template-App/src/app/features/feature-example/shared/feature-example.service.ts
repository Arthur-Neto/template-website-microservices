import {
    HttpClient,
    HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import {
    FeatureExampleAddCommand,
    FeatureExampleUpdateCommand,
    IFeatureExample,
} from './feature-example';

@Injectable({
    providedIn: 'root'
})
export class FeatureExampleService {

    private apiEndPoint: string;
    private httpOptions: any;

    public constructor(
        private http: HttpClient,
    ) {
        this.apiEndPoint = 'http://localhost:65264/api/feature-example';

        this.httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            })
        };
    }

    public getAll(): Observable<IFeatureExample[]> {

        return this.http.get<IFeatureExample[]>(this.apiEndPoint).pipe();
    }

    public getByID(id: number): Observable<IFeatureExample[]> {

        return this.http.get<IFeatureExample[]>(`${ this.apiEndPoint }/${ id }`).pipe();
    }

    public add(command: FeatureExampleAddCommand): Observable<boolean> {

        return this.http.post<boolean>(this.apiEndPoint, command).pipe();
    }

    public update(command: FeatureExampleUpdateCommand): Observable<boolean> {

        return this.http.put<boolean>(this.apiEndPoint, command).pipe();
    }

    public remove(id: number): Observable<boolean> {

        return this.http.delete<boolean>(`${ this.apiEndPoint }/${ id }`).pipe();
    }
}
