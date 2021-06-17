using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Template.Application
{
    public abstract class AppHandlerBase<TRepository> : IDisposable where TRepository : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        private AppHandlerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected AppHandlerBase(TRepository repository, IMapper mapper, ILogger logger)
            : this(logger)
        {
            _repository = repository;
            _mapper = mapper;
        }

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

        protected AppHandlerBase(TRepository repository, IMapper mapper, ILogger logger, IUnitOfWork unitOfWork)
            : this(repository, mapper, logger)
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
