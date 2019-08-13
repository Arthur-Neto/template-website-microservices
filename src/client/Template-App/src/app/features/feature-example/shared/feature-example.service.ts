import { Injectable } from '@angular/core';
import {
    Observable,
    of,
} from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FeatureExampleService {

    public constructor() { }

    public getFeatureExample(): Observable<string> {
        return of('Working');
    }
}
