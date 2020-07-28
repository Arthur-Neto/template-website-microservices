using System;
using System.Threading.Tasks;

namespace Template.Application
{
    public abstract class AppServiceBase<TRepository> : IDisposable where TRepository : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;

        protected AppServiceBase(TRepository repository)
        {
            _repository = repository;
        }

        protected AppServiceBase(IUnitOfWork unitOfWork, TRepository repository)
            : this(repository)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> CommitAsync()
        {
            return _unitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            if (_unitOfWork != null) { _unitOfWork.Dispose(); }
            _repository.Dispose();
        }
    }
}
