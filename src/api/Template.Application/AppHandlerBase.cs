using System;
using System.Threading.Tasks;
using AutoMapper;

namespace Template.Application
{
    public abstract class AppHandlerBase<TRepository> : IDisposable where TRepository : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;

        protected AppHandlerBase(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected AppHandlerBase(TRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : this(repository, mapper)
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
