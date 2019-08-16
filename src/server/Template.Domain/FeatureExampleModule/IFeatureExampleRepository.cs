using Template.Domain.CommonModule;

namespace Template.Domain.FeatureExampleModule
{
    public interface IFeatureExampleRepository :
        GetRepository<FeatureExample>,
        AddRepository<FeatureExample>,
        RemoveRepository<FeatureExample>,
        UpdateRepository<FeatureExample>
    { }
}
