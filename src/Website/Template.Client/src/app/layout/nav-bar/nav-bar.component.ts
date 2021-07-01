import { Component, Input } from '@angular/core';

import {
    IAuthenticatedUser,
    Role,
} from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

import { SideBarComponent } from '../side-bar/side-bar.component';

@Component({
    selector: 'app-nav-bar',
    templateUrl: './nav-bar.component.html',
    styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent {
    public isUserLoggedManager = false;

    @Input() sidebar: SideBarComponent | undefined;

    constructor(private authenticationService: AuthenticationService) {}

    public ngOnInit(): void {
        this.authenticationService.user.subscribe(
            (user: IAuthenticatedUser | null) => {
                this.isUserLoggedManager = user?.role === Role.Manager;
            }
        );
    }

    public onSidenavToggle() {
        this.sidebar?.toggle();
    }
}
