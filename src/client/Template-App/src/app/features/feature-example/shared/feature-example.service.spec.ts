import { TestBed } from '@angular/core/testing';

import { FeatureExampleService } from './feature-example.service';

describe('FeatureExampleService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('should be created', () => {
        const service: FeatureExampleService = TestBed.get(FeatureExampleService);
        expect(service).toBeTruthy();
    });
});
