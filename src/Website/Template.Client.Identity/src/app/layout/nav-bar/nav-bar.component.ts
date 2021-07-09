import { Component, Input } from '@angular/core';

import { SideBarComponent } from '../side-bar/side-bar.component';

@Component({
    selector: 'app-nav-bar',
    templateUrl: './nav-bar.component.html',
    styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent {
    public isUserLoggedManager = false;

    @Input() sidebar: SideBarComponent | undefined;

    public ngOnInit(): void {
        this.isUserLoggedManager = true;
    }

    public onSidenavToggle() {
        this.sidebar?.toggle();
    }
}
