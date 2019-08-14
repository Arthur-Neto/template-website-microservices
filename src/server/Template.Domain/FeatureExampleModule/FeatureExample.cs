using Template.Domain.CommonModule;

namespace Template.Domain.FeatureExampleModule
{
    public class FeatureExample : TEntity
    {
        public int ID { get; set; }
        public FeatureExampleEnum FeatureExampleType { get; private set; }
    }
}
