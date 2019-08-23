using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Template.Application.FeatureExampleModule.Models.Commands;
using Template.Domain.CommonModule;
using Template.Domain.FeatureExampleModule;
using Template.Infra.Data.Crosscutting.Guard;

namespace Template.Application.FeatureExampleModule
{
    public interface IFeatureExampleAppService
    {
        Task<IEnumerable<FeatureExample>> RetrieveAllAsync();
        Task<FeatureExample> RetrieveByIDAsync(int id);
        Task<bool> AddAsync(FeatureExampleAddCommand command);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateAsync(FeatureExampleUpdateCommand command);
    }

    public class FeatureExampleAppService : BaseAppService<IFeatureExampleRepository>, IFeatureExampleAppService
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IMapper> _mapper;

        public FeatureExampleAppService(
            Lazy<IUnitOfWork> unitOfWork,
            Lazy<IMapper> mapper,
            IFeatureExampleRepository repository
        )
            : base(repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(FeatureExampleAddCommand command)
        {
            var featureExample = _mapper.Value.Map<FeatureExample>(command);

            await Repository.AddAsync(featureExample);

            return await _unitOfWork.Value.CommitAsync() > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var featureExample = await Repository.GetByIDAsync(id);
            Guard.AgainstNull(featureExample, ExceptionArguments.NotFound);

            Repository.Remove(featureExample);

            return await _unitOfWork.Value.CommitAsync() > 0;
        }

        public async Task<IEnumerable<FeatureExample>> RetrieveAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async Task<FeatureExample> RetrieveByIDAsync(int id)
        {
            return await Repository.GetByIDAsync(id);
        }

        public async Task<bool> UpdateAsync(FeatureExampleUpdateCommand command)
        {
            var featureExample = await Repository.GetByIDAsync(command.ID);
            Guard.AgainstNull(featureExample, ExceptionArguments.NotFound);

            Repository.Update(featureExample);

            return await _unitOfWork.Value.CommitAsync() > 0;
        }
    }
}
