import { FeatureExample } from './feature-example.enum';

export interface IFeatureExample {
    id: number;
    enum: FeatureExample;
}

export class FeatureExampleAddCommand {
    featureExampleType: FeatureExample;
}

export class FeatureExampleUpdateCommand {
    id: number;
    featureExampleType: FeatureExample;
}
