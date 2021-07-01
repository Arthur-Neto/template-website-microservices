import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
    },
    {
        path: 'auth',
        loadChildren: () =>
            import('@modules/login/login.module').then((m) => m.LoginModule),
    },
    {
        path: 'dashboard',
        loadChildren: () =>
            import('@modules/dashboard/dashboard.module').then(
                (m) => m.DashboardModule
            ),
    },
    {
        path: 'users',
        loadChildren: () =>
            import('@modules/users/users.module').then((m) => m.UsersModule),
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
