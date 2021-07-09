import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { UsersApiService, UsersODataService } from './shared/users.service';
import { UserListComponent } from './users-list/users-list.component';
import { UsersRoutingModule } from './users.routing.module';

@NgModule({
    imports: [SharedModule, UsersRoutingModule],
    declarations: [UserListComponent],
    providers: [UsersApiService, UsersODataService],
})
export class UsersModule {}
