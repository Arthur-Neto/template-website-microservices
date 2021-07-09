import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard.routing.module';

@NgModule({
    imports: [SharedModule, DashboardRoutingModule],
    declarations: [DashboardComponent],
    exports: [DashboardComponent],
})
export class DashboardModule {}
