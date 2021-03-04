import { Component, OnInit } from '@angular/core';
import { IAuthenticatedUser, Role } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

@Component({
    selector: 'app-side-bar',
    templateUrl: './side-bar.component.html',
    styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit {
    public isUserLoggedManager = false;

    constructor(
        private authenticationService: AuthenticationService
    ) { }

    public ngOnInit(): void {
        this.authenticationService
            .user
            .subscribe((user: IAuthenticatedUser | null) => {
                this.isUserLoggedManager = user?.role === Role.Manager;
            });
    }
}
