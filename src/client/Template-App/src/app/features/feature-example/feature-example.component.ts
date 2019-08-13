import {
    Component,
    OnInit,
} from '@angular/core';
import { take } from 'rxjs/operators';

import { FeatureExampleService } from './shared/feature-example.service';

@Component({
    selector: 'app-feature-example',
    templateUrl: './feature-example.component.html',
    styleUrls: ['./feature-example.component.scss']
})
export class FeatureExampleComponent implements OnInit {

    public test: string;

    public constructor(
        private featureExampleService: FeatureExampleService,
    ) { }

    public ngOnInit() {
        this.featureExampleService
            .getFeatureExample()
            .pipe(take(1))
            .subscribe((result: string) => {
                this.test = result;
            });
    }
}
