import { NgModule } from '@angular/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SharedModule } from '@shared/shared.module';

import { SideBarComponent } from './side-bar.component';

@NgModule({
    imports: [
        SharedModule,

        MatSidenavModule,
        MatDividerModule,
    ],
    declarations: [
        SideBarComponent,
    ],
    exports: [
        SideBarComponent,
    ]
})
export class SideBarModule { }
