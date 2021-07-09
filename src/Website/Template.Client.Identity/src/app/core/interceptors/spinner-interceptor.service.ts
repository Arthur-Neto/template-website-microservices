import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { SpinnerService } from '@shared/components/spinner/spinner.service';

@Injectable()
export class SpinnerInterceptor implements HttpInterceptor {
    constructor(private readonly spinnerService: SpinnerService) {}

    public intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const spinnerSubscription: Subscription =
            this.spinnerService.spinner$.subscribe();

        return next
            .handle(req)
            .pipe(finalize(() => spinnerSubscription.unsubscribe()));
    }
}
