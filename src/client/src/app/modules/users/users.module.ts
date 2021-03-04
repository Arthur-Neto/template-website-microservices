import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { UsersApiService } from './shared/users.service';

@NgModule({
    imports: [
        SharedModule
    ],
    providers: [
        UsersApiService,
    ]
})
export class UsersModule { }
