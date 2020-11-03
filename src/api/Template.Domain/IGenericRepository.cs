using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Template.Domain
{
    public interface ICreateRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
    }

    public interface IRetrieveByIDRepository<TEntity, KeyType> : IDisposable where TEntity : class
    {
        Task<TEntity> RetrieveByIDAsync(KeyType key);
    }

    public interface IRetrieveAllRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> RetrieveAllAsync(Expression<Func<TEntity, bool>> expression = null, params Expression<Func<TEntity, object>>[] includeExpression);
    }

    public interface IUpdateRepository<TEntity> : IDisposable where TEntity : class
    {
        void Update(TEntity entity);
    }

    public interface IDeleteByIDRepository<TEntity, KeyType> : IDisposable where TEntity : class
    {
        Task DeleteAsync(KeyType key);
    }

    public interface ISingleOrDefaultRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params Expression<Func<TEntity, object>>[] includeExpression);
    }

    public interface ICountRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
    }
}
