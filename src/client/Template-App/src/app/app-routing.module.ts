import { NgModule } from '@angular/core';
import {
    RouterModule,
    Routes,
} from '@angular/router';

import { FeatureExampleComponent } from './features/feature-example/feature-example.component';

const routes: Routes = [
    { path: 'feature-example', component: FeatureExampleComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
