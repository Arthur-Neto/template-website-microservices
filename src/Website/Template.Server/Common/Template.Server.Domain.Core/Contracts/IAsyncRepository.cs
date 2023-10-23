using System.Linq.Expressions;

namespace Template.Server.Domain.Core.Contracts;

public interface ICreateRepository<TEntity> : IDisposable where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
}

public interface IRetrieveByIDRepository<TEntity> : IDisposable where TEntity : class
{
    Task<TEntity> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken);
}

public interface IRetrieveListByIDsRepository<TEntity> : IDisposable where TEntity : class
{
    Task<IQueryable<TEntity>> RetrieveByListOfIDAsync(List<Guid> ids, bool tracking, CancellationToken cancellationToken);
}

public interface IUpdateRepository<in TEntity> : IDisposable where TEntity : class
{
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}

public interface IDeleteRepository<in TEntity> : IDisposable where TEntity : class
{
    Task Delete(TEntity entity, CancellationToken cancellationToken);
}

public interface ISingleOrDefaultRepository<TEntity> : IDisposable where TEntity : class
{
    Task<TEntity> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool tracking,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    );
}

public interface IFirstOrDefaultRepository<TEntity> : IDisposable where TEntity : class
{
    Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool tracking,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    );
}

public interface ICountRepository<TEntity> : IDisposable where TEntity : class
{
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    );

    Task<int> CountAsync(CancellationToken cancellationToken);
}

public interface IAnyRepository<TEntity> : IDisposable where TEntity : class
{
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    );
}

public interface IRetrieveByExpressionRepository<TEntity> : IDisposable where TEntity : class
{
    Task<List<TEntity>> RetrieveByExpressionAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    );
}

public interface IRetrieveAllRepository<TEntity> : IDisposable where TEntity : class
{
    Task<List<TEntity>> RetrieveAllAsync(CancellationToken cancellationToken);
}
