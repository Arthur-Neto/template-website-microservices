import {
    Component,
    OnInit,
} from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
} from '@angular/forms';

import { take } from 'rxjs/operators';

import { FeatureExampleService } from './shared/feature-example.service';

@Component({
    selector: 'app-feature-example',
    templateUrl: './feature-example.component.html',
    styleUrls: ['./feature-example.component.scss']
})
export class FeatureExampleComponent implements OnInit {

    public getAllResult: any;
    public getByIDResult: any;
    public removeResult: any;
    public addResult: any;
    public updateResult: any;

    public getByIDFormGroup: FormGroup;
    public addFormGroup: FormGroup;
    public updateFormGroup: FormGroup;
    public removeFormGroup: FormGroup;

    public constructor(
        private featureExampleService: FeatureExampleService,
        private fb: FormBuilder,
    ) { }

    public ngOnInit() {
        this.getByIDFormGroup = this.fb.group({
            id: ['', [Validators.required, Validators.min(1)]],
        });

        this.addFormGroup = this.fb.group({
            enum: ['', [Validators.required, Validators.min(1)]],
        });

        this.updateFormGroup = this.fb.group({
            id: ['', [Validators.required, Validators.min(1)]],
            enum: ['', [Validators.required, Validators.min(1)]],
        });

        this.removeFormGroup = this.fb.group({
            id: ['', [Validators.required, Validators.min(1)]],
        });
    }

    public onGetAll() {
        this.featureExampleService
            .getAll()
            .pipe(take(1))
            .subscribe((result: any) => {
                this.getAllResult = result;
            });
    }

    public onGetByID() {
        this.featureExampleService
            .getByID(this.getByIDFormGroup.value.id)
            .pipe(take(1))
            .subscribe((result: any) => {
                this.getByIDResult = result;
            });
    }

    public onRemove() {
        this.featureExampleService
            .remove(this.removeFormGroup.value.id)
            .pipe(take(1))
            .subscribe((result: any) => {
                this.removeResult = result;
            });
    }

    public onAdd() {
        const command: any = {
            featureExampleType: this.addFormGroup.value.enum
        };

        this.featureExampleService
            .add(command)
            .pipe(take(1))
            .subscribe((result: any) => {
                this.addResult = result;
            });
    }

    public onUpdate() {
        const command: any = {
            id: this.updateFormGroup.value.id,
            featureExampleType: this.updateFormGroup.value.enum
        };

        this.featureExampleService
            .update(command)
            .pipe(take(1))
            .subscribe((result: any) => {
                this.updateResult = result;
            });
    }
}
