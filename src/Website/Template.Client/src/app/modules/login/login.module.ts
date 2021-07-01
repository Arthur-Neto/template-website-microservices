import { NgModule } from '@angular/core';

import { UsersApiService } from '@modules/users/shared/users.service';

import { SharedModule } from '@shared/shared.module';

import { LoginCreateComponent } from './login-create/login-create.component';
import { LoginEditComponent } from './login-edit/login-edit.component';
import { LoginComponent } from './login.component';
import { LoginRoutingModule } from './login.routing.module';

@NgModule({
    imports: [SharedModule, LoginRoutingModule],
    declarations: [LoginComponent, LoginCreateComponent, LoginEditComponent],
    exports: [LoginComponent],
    providers: [UsersApiService],
})
export class LoginModule {}
