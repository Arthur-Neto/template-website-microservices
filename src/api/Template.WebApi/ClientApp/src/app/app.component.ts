import { isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
    static isBrowser = new BehaviorSubject<boolean>(null);

    constructor(
        private router: Router,
        @Inject(PLATFORM_ID) private platformId: any
    ) {
        AppComponent.isBrowser.next(isPlatformBrowser(platformId));
    }

    public ngOnInit(): void {
        this.router.navigate(['']);
    }
}
