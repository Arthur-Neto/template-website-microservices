import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticateCommand } from '@core/authentication/authentication-models';
import { AuthenticationService } from '@core/authentication/authentication.service';

import { take } from 'rxjs/operators';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    public form: FormGroup;

    public get showUsernameRequiredError(): boolean {
        return this.form.controls['username'].hasError('required');
    }
    public get showUsernameNotFoundError(): boolean {
        return this.form.controls['username'].hasError('doesntExist');
    }
    public get showPasswordRequiredError(): boolean {
        return this.form.controls['password'].hasError('required');
    }
    public get showWrongPasswordError(): boolean {
        return this.form.controls['password'].hasError('wrongPassword');
    }
    public get showConfirmPasswordRequiredError(): boolean {
        return this.form.controls['confirmPassword'].hasError('required');
    }
    public get showPasswordDoesntMatchError(): boolean {
        return this.form.controls['confirmPassword'].hasError('pattern');
    }

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService,
    ) {
        if (this.authenticationService.userValue) {
            this.router.navigate(['dashboard']);
        }
    }

    public ngOnInit(): void {
        this.form = this.fb.group({
            username: [null, Validators.required],
            password: ['', Validators.required],
            confirmPassword: ['', Validators.required],
        });
    }

    public onSubmit() {
        if (this.form.valid) {
            const command = <AuthenticateCommand>{
                username: this.form.controls['username'].value,
                password: this.form.controls['password'].value
            };

            this.authenticationService
                .login(command)
                .pipe(take(1))
                .subscribe({
                    next: this.onSuccessCallback.bind(this),
                    error: this.onErrorCallback.bind(this)
                });
        }
    }

    public onCreateNewUser() {
        this.router.navigate(['../create'], { relativeTo: this.route });
    }

    private onSuccessCallback(): void {
        this.router.navigate(['dashboard']);
    }

    private onErrorCallback(error: string): void {
        if (error.match('NotFound')) {
            this.form.controls['username'].setErrors({ doesntExist: true });
        } else if (error.match('IncorrectUserPassword')) {
            this.form.controls['password'].setErrors({ wrongPassword: true });
        }
    }
}
