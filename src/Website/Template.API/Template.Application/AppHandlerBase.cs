using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Template.Domain;
using Template.Infra.Data.EF.Contexts;

namespace Template.Application
{
    public abstract class AppHandlerBase<TRepository, DbContext> : AppHandlerBase<TRepository>
        where TRepository : IDisposable
        where DbContext : IDatabaseContext
    {
        private readonly IUnitOfWork<DbContext> _unitOfWork;

        protected AppHandlerBase(TRepository repository, IMapper mapper, IUnitOfWork<DbContext> unitOfWork)
            : base(repository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        protected AppHandlerBase(TRepository repository, IMapper mapper, ILogger logger, IUnitOfWork<DbContext> unitOfWork)
            : base(repository, mapper, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> CommitAsync()
        {
            return _unitOfWork.CommitAsync();
        }

        public new void Dispose()
        {
            if (_unitOfWork != null) { _unitOfWork.Dispose(); }

            base.Dispose();
        }
    }

    public abstract class AppHandlerBase<TRepository> : IDisposable
        where TRepository : IDisposable
    {
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        private AppHandlerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected AppHandlerBase(TRepository repository)
        {
            _repository = repository;
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

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
