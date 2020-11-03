using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Domain;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.Repositories
{
    public sealed class GenericRepository<TEntity, KeyType> :
        ICreateRepository<TEntity>,
        IRetrieveByIDRepository<TEntity, KeyType>,
        IRetrieveOData<TEntity>,
        IUpdateRepository<TEntity>,
        IDeleteByIDRepository<TEntity, KeyType>,
        ISingleOrDefaultRepository<TEntity>,
        ICountRepository<TEntity> where TEntity : class
    {
        public IDatabaseContext Context { get; private set; }

        public GenericRepository(IDatabaseContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entityEntry = await Context.Set<TEntity>().AddAsync(entity);

            return entityEntry.Entity;
        }

        public async Task<TEntity> RetrieveByIDAsync(KeyType key)
        {
            return await Context.Set<TEntity>().FindAsync(key);
        }

        public IQueryable<TEntity> RetrieveOData()
        {
            return Context.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(KeyType key)
        {
            var entities = Context.Set<TEntity>();
            var entity = await entities.FindAsync(key);
            entities.Remove(entity);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params Expression<Func<TEntity, object>>[] includeExpression)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includeExpression.Length == 0)
            {
                if (tracking)
                {
                    return await query.SingleOrDefaultAsync(expression);
                }

                return await query.AsNoTracking().SingleOrDefaultAsync(expression);
            }

            if (tracking)
            {
                return await includeExpression.Aggregate(query, (current, include) => current.Include(include)).SingleOrDefaultAsync(expression);
            }

            return await includeExpression.Aggregate(query, (current, include) => current.AsNoTracking().Include(include)).SingleOrDefaultAsync(expression);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().CountAsync(expression);
        }
    }
}
