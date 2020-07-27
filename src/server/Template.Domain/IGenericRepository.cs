using System;
using System.Collections.Generic;
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
        Task<IEnumerable<TEntity>> RetrieveAllAsync();
    }

    public interface IUpdateRepository<TEntity> : IDisposable where TEntity : class
    {
        void Update(TEntity entity);
    }

    public interface IDeleteByIDRepository<TEntity, KeyType> : IDisposable where TEntity : class
    {
        Task DeleteAsync(KeyType key);
    }
}
