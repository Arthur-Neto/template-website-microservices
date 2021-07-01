import { NgModule } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';

import { SharedModule } from '@shared/shared.module';

import { SideBarComponent } from './side-bar.component';

@NgModule({
    imports: [SharedModule, MatSidenavModule],
    declarations: [SideBarComponent],
    exports: [SideBarComponent],
})
export class SideBarModule {}
