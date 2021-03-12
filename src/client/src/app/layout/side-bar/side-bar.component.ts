import { Component, OnInit } from '@angular/core';
import {
    Event, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router
} from '@angular/router';
import { IAuthenticatedUser, Role } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

@Component({
    selector: 'app-side-bar',
    templateUrl: './side-bar.component.html',
    styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit {
    public isLoading = true;
    public isUserLoggedManager = false;

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
}
