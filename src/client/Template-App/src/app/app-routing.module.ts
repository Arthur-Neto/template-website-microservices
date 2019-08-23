import { NgModule } from '@angular/core';
import {
    RouterModule,
    Routes,
} from '@angular/router';

import { FeatureExampleComponent } from './features/feature-example/feature-example.component';

const routes: Routes = [
    {
        path: 'feature-example', component: FeatureExampleComponent,
        loadChildren: () => import('./features/feature-example/feature-example.module').then((mod) => mod.FeatureExampleModule)
    },
    {
        path: '',
        redirectTo: '',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
