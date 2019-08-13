import {
    Component,
    OnInit,
} from '@angular/core';

import { FeatureExampleService } from './shared/feature-example.service';

@Component({
    selector: 'app-feature-example',
    templateUrl: './feature-example.component.html',
    styleUrls: ['./feature-example.component.scss']
})
export class FeatureExampleComponent implements OnInit {

    public constructor(
        private featureExampleService: FeatureExampleService,
    ) { }

    public ngOnInit() {
        this.featureExampleService
            .getFeatureExample()
            .pipe(
                take(1))
            .subscribe(() => {
                console.log('Working');
            });
    }
}
