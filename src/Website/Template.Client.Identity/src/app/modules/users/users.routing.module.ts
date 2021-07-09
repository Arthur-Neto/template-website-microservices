import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '@core/guards/authentication.guard';

import { UserListComponent } from './users-list/users-list.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/users/list',
        pathMatch: 'full',
    },
    {
        path: '',
        children: [
            {
                path: 'list',
                canActivate: [AuthGuard],
                component: UserListComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UsersRoutingModule {}
