using System;
using Template.Domain;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.Repositories
{
    public abstract class GenericRepositoryBase<TEntity> : IDisposable where TEntity : Entity
    {
        protected GenericRepository<TEntity> GenericRepository;

        protected GenericRepositoryBase(IDatabaseContext context)
        {
            GenericRepository = new GenericRepository<TEntity>(context);
        }

        public void Dispose()
        {
            GenericRepository.Dispose();
        }
    }
}
