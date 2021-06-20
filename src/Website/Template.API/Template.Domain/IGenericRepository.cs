using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Template.Domain
{
    public interface ICreateRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    }

    public interface IRetrieveByIDRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken);
    }

    public interface IRetrieveOData : IDisposable
    {
        IQueryable<TDestination> RetrieveOData<TDestination>(IMapper mapper);
    }

    public interface IUpdateRepository<TEntity> : IDisposable where TEntity : class
    {
        void Update(TEntity entity);
    }

    public interface IDeleteRepository<TEntity> : IDisposable where TEntity : class
    {
        void Delete(TEntity entity);
    }

    public interface ISingleOrDefaultRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeExpression);
    }

    public interface ICountRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    }

    public interface IAnyRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    }
}
