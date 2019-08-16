using Template.Domain.FeatureExampleModule;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.Repositories.FeaturesExamplesModule
{
    public class FeaturesExamplesRepository : GenericRepository<FeatureExample>, IFeatureExampleRepository
    {
        public FeaturesExamplesRepository(ExampleContext context)
            : base(context)
        { }
    }
}
