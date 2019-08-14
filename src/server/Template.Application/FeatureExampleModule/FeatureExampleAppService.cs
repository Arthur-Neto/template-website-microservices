using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.FeatureExampleModule;

namespace Template.Application.FeatureExampleModule
{
    public interface IFeatureExampleAppService
    {
        Task<IEnumerable<FeatureExample>> RetrieveAllAsync();
    }

    public class FeatureExampleAppService : BaseAppService<IFeatureExampleRepository>, IFeatureExampleAppService
    {
        public FeatureExampleAppService(
            IFeatureExampleRepository repository
        )
            : base(repository)
        { }

        public async Task<IEnumerable<FeatureExample>> RetrieveAllAsync()
        {
            return await Repository.GetAll();
        }
    }
}
