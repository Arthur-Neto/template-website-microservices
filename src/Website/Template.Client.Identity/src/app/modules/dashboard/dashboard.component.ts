import { ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

import { IDayAndMonth } from '@shared/components/carousel-daypicker/carousel-daypicker.component';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnDestroy {
    public isLoading = true;
    public dayAndMonth!: IDayAndMonth;

    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(private cdr: ChangeDetectorRef) {}

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
