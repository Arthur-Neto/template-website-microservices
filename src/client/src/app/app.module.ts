import { LayoutModule } from '@angular/cdk/layout';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@core/core.module';
import { CustomLayoutModule } from '@layout/layout.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorInterceptor } from './core/interceptors/error-interceptor.service';
import { JwtInterceptor } from './core/interceptors/jwt-interceptor.service';
import { SpinnerInterceptor } from './core/interceptors/spinner.interceptor.service';

@NgModule({
    imports: [
        HttpClientModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        LayoutModule,

        CoreModule,
        CustomLayoutModule,
    ],
    providers: [
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 5000 } },
        { provide: STEPPER_GLOBAL_OPTIONS, useValue: { showError: true } },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: SpinnerInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true,
        },
    ],
    declarations: [AppComponent],
    bootstrap: [AppComponent],
})
export class AppModule { }
