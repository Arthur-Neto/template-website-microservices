using Template.Server.Domain.Core;
using Template.Server.Infra.EF.Core.Contracts;

namespace Template.Server.Infra.EF.Core;

public abstract class GenericEntityRepositoryBase<TEntity> : IDisposable where TEntity : Entity
{
    protected GenericEntityRepository<TEntity> GenericRepository;

    protected GenericEntityRepositoryBase(IDatabaseContext context)
    {
        GenericRepository = new GenericEntityRepository<TEntity>(context);
    }

    public void Dispose()
    {
        GenericRepository.Dispose();
    }
}

public abstract class GenericRepositoryBase<TEntity> : IDisposable where TEntity : class
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
