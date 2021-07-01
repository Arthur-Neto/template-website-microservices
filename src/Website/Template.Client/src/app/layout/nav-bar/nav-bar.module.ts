import { NgModule } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { SharedModule } from '@shared/shared.module';

import { NavBarLoginComponent } from './nav-bar-login/nav-bar-login.component';
import { NavBarComponent } from './nav-bar.component';

@NgModule({
    imports: [SharedModule, MatSidenavModule, MatToolbarModule, MatMenuModule],
    declarations: [NavBarComponent, NavBarLoginComponent],
    exports: [NavBarComponent],
})
export class NavBarModule {}
