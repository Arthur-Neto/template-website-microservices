import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import {
    Event, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router
} from '@angular/router';
import { IAuthenticatedUser, Role } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

import { animateText, onMainContentChange, onSideNavChange } from '../../animations/animations';

@Component({
    selector: 'app-side-bar',
    templateUrl: './side-bar.component.html',
    styleUrls: ['./side-bar.component.scss'],
    animations: [onSideNavChange, onMainContentChange, animateText]
})
export class SideBarComponent implements OnInit {
    public isLoading = true;
    public isExpanded = true;
    public isUserLoggedManager = false;

    @ViewChild('sidenav') sidenav!: MatSidenav;

    constructor(
        private authenticationService: AuthenticationService,
        private router: Router,
    ) {
        this.router.events.subscribe((event: Event) => {
            switch (true) {
                case event instanceof NavigationStart: {
                    this.isLoading = true;
                    break;
                }
                case event instanceof NavigationEnd:
                case event instanceof NavigationCancel:
                case event instanceof NavigationError: {
                    this.isLoading = false;
                    break;
                }
                default: {
                    this.isLoading = false;
                    break;
                }
            }
        });
    }

    public ngOnInit(): void {
        this.authenticationService
            .user
            .subscribe((user: IAuthenticatedUser | null) => {
                this.isUserLoggedManager = user?.role === Role.Manager;
            });
    }

    public toggle() {
        this.isExpanded = !this.isExpanded;
    }
}
