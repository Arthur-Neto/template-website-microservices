using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Template.Domain;
using Template.Infra.Data.EF.Contexts;

namespace Template.Infra.Data.EF.Repositories
{
    public sealed class GenericRepository<TEntity> :
        ICreateRepository<TEntity>,
        IRetrieveByIDRepository<TEntity>,
        IRetrieveOData,
        IUpdateRepository<TEntity>,
        IDeleteRepository<TEntity>,
        ISingleOrDefaultRepository<TEntity>,
        ICountRepository<TEntity>,
        IAnyRepository<TEntity>
        where TEntity : Entity
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

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var entityEntry = await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

            return entityEntry.Entity;
        }

        public Task<TEntity> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(p => p.ID == id, cancellationToken);
        }

        public IQueryable<TDestination> RetrieveOData<TDestination>(IMapper mapper)
        {
            return Context.Set<TEntity>()
                          .AsNoTracking()
                          .ProjectTo<TDestination>(mapper.ConfigurationProvider);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeExpression)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includeExpression.Length == 0)
            {
                if (tracking)
                {
                    return query.SingleOrDefaultAsync(expression, cancellationToken);
                }

                return query.AsNoTracking().SingleOrDefaultAsync(expression, cancellationToken);
            }

            if (tracking)
            {
                return includeExpression.Aggregate(query, (current, include) => current.Include(include)).SingleOrDefaultAsync(expression, cancellationToken);
            }

            return includeExpression.Aggregate(query, (current, include) => current.AsNoTracking().Include(include)).SingleOrDefaultAsync(expression, cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().AsNoTracking().CountAsync(expression, cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().AsNoTracking().AnyAsync(expression, cancellationToken);
        }
    }
}
