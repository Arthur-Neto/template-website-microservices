import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import {
    Event,
    NavigationCancel,
    NavigationEnd,
    NavigationError,
    NavigationStart,
    Router,
} from '@angular/router';

import {
    IAuthenticatedUser,
    Role,
} from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';
import { LocalStorageService } from '@core/local-storage/local-storage.service';

import {
    animateText,
    onMainContentChange,
    onSideNavChange,
} from './side-bar.animations';
import { SidebarState } from './side-bar.model';

@Component({
    selector: 'app-side-bar',
    templateUrl: './side-bar.component.html',
    styleUrls: ['./side-bar.component.scss'],
    animations: [onSideNavChange, onMainContentChange, animateText],
})
export class SideBarComponent implements OnInit, AfterViewInit {
    public isLoading = true;
    public sidebarState = SidebarState.HIDDEN;
    public isUserLoggedManager = false;

    public get isExpanded(): boolean {
        return this.sidebarState === SidebarState.OPEN;
    }

    @ViewChild('sidenav') sidenav!: MatSidenav;

    constructor(
        private authenticationService: AuthenticationService,
        private router: Router,
        private localStorageService: LocalStorageService,
        private cdr: ChangeDetectorRef
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

    public ngAfterViewInit(): void {
        if (this.isUserLoggedManager) {
            const sidebarState =
                this.localStorageService.getItem('sidebarState');
            this.sidebarState = sidebarState
                ? (sidebarState as SidebarState)
                : SidebarState.OPEN;
        }

        this.cdr.detectChanges();
    }

    public ngOnInit(): void {
        this.authenticationService.user.subscribe(
            (user: IAuthenticatedUser | null) => {
                this.isUserLoggedManager = user?.role === Role.Manager;
                this.sidebarState = this.isUserLoggedManager
                    ? SidebarState.CLOSE
                    : SidebarState.HIDDEN;
            }
        );
    }

    public toggle() {
        if (this.sidebarState === SidebarState.OPEN) {
            this.sidebarState = SidebarState.CLOSE;
        } else {
            this.sidebarState = SidebarState.OPEN;
        }

        this.localStorageService.setItem('sidebarState', this.sidebarState);
    }
}
