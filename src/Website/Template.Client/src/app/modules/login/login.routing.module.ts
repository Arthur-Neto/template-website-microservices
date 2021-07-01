import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '@core/guards/authentication.guard';

import { LoginCreateComponent } from './login-create/login-create.component';
import { LoginEditComponent } from './login-edit/login-edit.component';
import { LoginComponent } from './login.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/auth/login',
        pathMatch: 'full',
    },
    {
        path: '',
        children: [
            {
                path: 'login',
                component: LoginComponent,
            },
            {
                path: 'create',
                component: LoginCreateComponent,
            },
            {
                path: 'edit',
                canActivate: [AuthGuard],
                component: LoginEditComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LoginRoutingModule {}
