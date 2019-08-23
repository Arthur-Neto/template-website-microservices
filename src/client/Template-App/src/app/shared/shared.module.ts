import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material.module';
import { SpinnerModule } from './spinner-component/spinner.module';

@NgModule({
    declarations: [],
    imports: [
        CommonModule
    ],
    exports: [
        MaterialModule,
        SpinnerModule,
    ],
})
export class SharedModule { }
