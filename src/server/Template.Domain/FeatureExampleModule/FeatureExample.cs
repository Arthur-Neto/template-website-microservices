using Template.Domain.CommonModule;

namespace Template.Domain.FeatureExampleModule
{
    public class FeatureExample : Entity
    {
        public FeatureExampleEnum FeatureExampleType { get; private set; }

        public FeatureExample()
        { }

        public FeatureExample(FeatureExampleEnum featureExampleType)
        {
            FeatureExampleType = featureExampleType;
        }
    }
}
