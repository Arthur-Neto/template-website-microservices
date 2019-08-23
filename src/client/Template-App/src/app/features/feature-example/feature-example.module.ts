import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
    FormsModule,
    ReactiveFormsModule,
} from '@angular/forms';

import { SharedModule } from '../../shared/shared.module';
import { FeatureExampleRoutingModule } from './feature-example-routing.module';
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
        CommonModule,
        FeatureExampleRoutingModule,
        SharedModule,
        FormsModule,
        ReactiveFormsModule,
    ],
    exports: [],
})
export class FeatureExampleModule { }
