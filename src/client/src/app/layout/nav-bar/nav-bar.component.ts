import { Component, Input } from '@angular/core';

import { SideBarComponent } from '../side-bar/side-bar.component';

@Component({
    selector: 'app-nav-bar',
    templateUrl: './nav-bar.component.html',
    styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent {
    @Input() sidebar: SideBarComponent | undefined;

    public onSidenavToggle() {
        this.sidebar?.toggle()
    }
}
