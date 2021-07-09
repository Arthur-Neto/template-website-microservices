import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { LayoutContentComponent } from './layout-content/layout-content.component';
import { NavBarModule } from './nav-bar/nav-bar.module';
import { SideBarModule } from './side-bar/side-bar.module';

@NgModule({
    imports: [SharedModule, NavBarModule, SideBarModule],
    declarations: [LayoutContentComponent],
    exports: [LayoutContentComponent],
})
export class CustomLayoutModule {}
