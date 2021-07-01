import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';

import { IAuthenticateCommand } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
    public form!: FormGroup;

    public get showUsernameRequiredError(): boolean {
        return this.form.controls.username.hasError('required');
    }
    public get showUsernameNotFoundError(): boolean {
        return this.form.controls.username.hasError('doesntExist');
    }
    public get showPasswordRequiredError(): boolean {
        return this.form.controls.password.hasError('required');
    }
    public get showWrongPasswordError(): boolean {
        return this.form.controls.password.hasError('wrongPassword');
    }

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService
    ) {
        if (this.authenticationService.userValue) {
            this.router.navigate(['dashboard']);
        }
    }

    public ngOnInit(): void {
        this.form = this.fb.group({
            username: [null, Validators.required],
            password: ['', Validators.required],
        });
    }

    public onSubmit(): void {
        if (this.form.valid) {
            const command = {
                username: this.form.controls.username.value,
                password: this.form.controls.password.value,
            } as IAuthenticateCommand;

            this.authenticationService
                .login(command)
                .pipe(take(1))
                .subscribe({
                    next: this.onSuccessCallback.bind(this),
                    error: this.onErrorCallback.bind(this),
                });
        }
    }

    public onCreateNewUser(): void {
        this.router.navigate(['../create'], { relativeTo: this.route });
    }

    private onSuccessCallback(): void {
        this.router.navigate(['dashboard']);
    }

    private onErrorCallback(error: any): void {
        if (error.match('NotFound')) {
            this.form.controls.username.setErrors({ doesntExist: true });
        } else if (error.match('IncorrectUserPassword')) {
            this.form.controls.password.setErrors({ wrongPassword: true });
        }
    }
}
