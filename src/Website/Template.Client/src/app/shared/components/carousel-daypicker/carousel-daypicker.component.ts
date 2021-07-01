import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import * as moment from 'moment';

interface IDayOnWeek {
    dayName: string;
    dayAndMonth: string;
    dayBeforeToday: boolean;
}

export interface IDayAndMonth {
    day: string;
    month: string;
}

@Component({
    selector: 'app-carousel-daypicker',
    templateUrl: './carousel-daypicker.component.html',
    styleUrls: ['./carousel-daypicker.component.scss'],
})
export class CarouselDaypickerComponent implements OnInit {
    @Output() dayChanged: EventEmitter<IDayAndMonth> =
        new EventEmitter<IDayAndMonth>();

    public daysOnWeek: IDayOnWeek[] = [];
    public isCurrentWeek = false;

    private weekNumber = 0;

    public ngOnInit(): void {
        this.changeWeek(this.weekNumber);
    }

    public onWeekChange(nextWeek: boolean): void {
        nextWeek
            ? this.changeWeek(++this.weekNumber)
            : this.changeWeek(--this.weekNumber);
    }

    public onDayChange(dayOnWeek: IDayOnWeek): void {
        const split: string[] = dayOnWeek.dayAndMonth.split('/');
        const dayAndMonth: IDayAndMonth = { day: split[0], month: split[1] };

        this.dayChanged.emit(dayAndMonth);
    }

    private changeWeek(weekNumber: number): void {
        this.isCurrentWeek = false;
        this.daysOnWeek = [];
        const today = new Date();
        const startOfWeek: moment.Moment = moment()
            .add(weekNumber, 'weeks')
            .startOf('isoWeek');

        for (let i = 0; i <= 6; i++) {
            const dayAndMonth: string = moment(startOfWeek)
                .add(i, 'days')
                .format('DD/MM');

            let dayBeforeToday = false;
            let dayName: string;
            const day: moment.Moment = moment(startOfWeek).add(i, 'days');
            if (day.isSame(today, 'day')) {
                dayName = 'TODAY';
                this.isCurrentWeek = true;
                this.onDayChange({
                    dayName,
                    dayAndMonth,
                    dayBeforeToday: false,
                });
            } else {
                dayName = day.format('ddd').toUpperCase();

                if (day.isBefore(today, 'day')) {
                    dayBeforeToday = true;
                }
            }

            this.daysOnWeek.push({ dayName, dayAndMonth, dayBeforeToday });
        }
    }
}
