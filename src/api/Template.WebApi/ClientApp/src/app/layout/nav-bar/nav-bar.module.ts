import { NgModule } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { SharedModule } from '@shared/shared.module';

import { NavBarLoginComponent } from './nav-bar-login/nav-bar-login.component';
import { NavBarComponent } from './nav-bar.component';

@NgModule({
    imports: [
        SharedModule,

        MatToolbarModule,
        MatMenuModule,
    ],
    declarations: [
        NavBarComponent,
        NavBarLoginComponent,
    ],
    exports: [
        NavBarComponent
    ],
})
export class NavBarModule { }
