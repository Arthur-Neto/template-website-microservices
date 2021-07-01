import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { IAuthenticatedUser } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

import { IDayAndMonth } from '@shared/components/carousel-daypicker/carousel-daypicker.component';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit, OnDestroy {
    public isLoading = true;
    public dayAndMonth!: IDayAndMonth;
    public userLogged!: IAuthenticatedUser | null;

    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(
        private authenticationService: AuthenticationService,
        private cdr: ChangeDetectorRef
    ) {}

    public ngOnInit(): void {
        this.authenticationService.user
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((user: IAuthenticatedUser | null) => {
                this.userLogged = user;
            });
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public onDayChanged(dayAndMonth: IDayAndMonth): void {
        this.isLoading = true;
        this.dayAndMonth = dayAndMonth;
        this.isLoading = false;
        this.cdr.detectChanges();
    }
}
