import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FeatureExampleModule } from './features/feature-example/feature-example.module';

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FeatureExampleModule,
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
