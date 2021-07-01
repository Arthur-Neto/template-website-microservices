import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';

import { IUserCreateCommand } from '@modules/users/shared/users.model';
import { UsersApiService } from '@modules/users/shared/users.service';

@Component({
    templateUrl: './login-create.component.html',
    styleUrls: ['./login-create.component.scss'],
})
export class LoginCreateComponent implements OnInit {
    public form!: FormGroup;

    public get showUsernameRequiredError(): boolean {
        return this.form.controls.username.hasError('required');
    }
    public get showUsernameDuplicatedError(): boolean {
        return this.form.controls.username.hasError('duplicated');
    }
    public get showPasswordRequiredError(): boolean {
        return this.form.controls.password.hasError('required');
    }
    public get showConfirmPasswordRequiredError(): boolean {
        return this.form.controls.confirmPassword.hasError('required');
    }
    public get showPasswordDoesntMatchError(): boolean {
        return this.form.controls.confirmPassword.hasError('pattern');
    }

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private usersApiService: UsersApiService,
        private snackBar: MatSnackBar
    ) {}

    public ngOnInit(): void {
        this.form = this.fb.group({
            username: [null, Validators.required],
            password: [null, Validators.required],
            confirmPassword: [null, Validators.required],
        });
    }

    public onSubmit(): void {
        if (this.form.valid) {
            const command = {
                username: this.form.controls.username.value,
                password: this.form.controls.password.value,
            } as IUserCreateCommand;

            this.usersApiService
                .create(command)
                .pipe(take(1))
                .subscribe({
                    next: this.onSuccessCallback.bind(this),
                    error: this.onErrorCallback.bind(this),
                });
        }
    }

    private onSuccessCallback(): void {
        this.snackBar.open('Create success');
        this.router.navigate(['auth/login']);
    }

    private onErrorCallback(error: string): void {
        if (error.match('Duplicating')) {
            this.form.controls.username.setErrors({ duplicated: true });
        }
    }
}
