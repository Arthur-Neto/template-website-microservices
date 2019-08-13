import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FeatureExampleComponent } from './feature-example.component';
import { FeatureExampleService } from './shared/feature-example.service';

@NgModule({
    providers: [
        FeatureExampleService,
    ],
    declarations: [
        FeatureExampleComponent,
    ],
    imports: [
        CommonModule
    ],
    exports: [],
})
export class FeatureExampleModule { }
