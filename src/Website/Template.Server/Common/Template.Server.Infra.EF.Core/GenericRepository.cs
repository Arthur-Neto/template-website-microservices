using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Template.Server.Domain.Core;
using Template.Server.Domain.Core.Contracts;
using Template.Server.Infra.EF.Core.Contracts;

namespace Template.Server.Infra.EF.Core;

public class GenericRepository<TEntity>
    : ICreateRepository<TEntity>,
      IUpdateRepository<TEntity>,
      IDeleteRepository<TEntity>,
      ISingleOrDefaultRepository<TEntity>,
      IFirstOrDefaultRepository<TEntity>,
      ICountRepository<TEntity>,
      IAnyRepository<TEntity>,
      IRetrieveByExpressionRepository<TEntity>
    where TEntity : class
{
    public IDatabaseContext Context { get; }

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
        EntityEntry<TEntity> entityEntry = await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

        return entityEntry.Entity;
    }

    public Task Delete(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => Context.Set<TEntity>().Remove(entity), cancellationToken);
    }

    public Task<TEntity> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        bool tracking,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    )
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();

        return includeExpression.Length == 0
            ? tracking
                ? query.SingleOrDefaultAsync(expression, cancellationToken)
                : query.AsNoTracking().SingleOrDefaultAsync(expression, cancellationToken)
            : tracking
            ? includeExpression.Aggregate(
                query,
                (current, include) => current.Include(include)
            )
            .SingleOrDefaultAsync(expression, cancellationToken)
            : includeExpression.Aggregate(
                query,
                (current, include) => current.AsNoTracking().Include(include)
            )
            .SingleOrDefaultAsync(expression, cancellationToken);
    }

    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeExpression)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();

        return includeExpression.Length == 0
            ? tracking
                ? query.FirstOrDefaultAsync(expression, cancellationToken)
                : query.AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken)
            : tracking
            ? includeExpression.Aggregate(
                query,
                (current, include) => current.Include(include)
            )
            .FirstOrDefaultAsync(expression, cancellationToken)
            : includeExpression.Aggregate(
                query,
                (current, include) => current.AsNoTracking().Include(include)
            )
            .FirstOrDefaultAsync(expression, cancellationToken);
    }

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeExpression
    )
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        return includeExpression.Length == 0
            ? query.CountAsync(expression, cancellationToken)
            : includeExpression.Aggregate(
                query,
                (current, include) => current.Include(include)
            )
            .CountAsync(expression, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        return query.CountAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeExpression)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        return includeExpression.Length == 0
            ? query.AnyAsync(expression, cancellationToken)
            : includeExpression.Aggregate(
                query,
                (current, include) => current.Include(include)
            )
            .AnyAsync(expression, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => Context.Entry(entity).State = EntityState.Modified, cancellationToken);
    }

    public Task<List<TEntity>> RetrieveByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeExpression)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        return includeExpression.Length == 0
            ? query.Where(expression).ToListAsync(cancellationToken)
            : includeExpression.Aggregate(
                query,
                (current, include) => current.Include(include)
            )
            .Where(expression)
            .ToListAsync(cancellationToken);
    }
}

public sealed class GenericEntityRepository<TEntity> :
    GenericRepository<TEntity>,
    IRetrieveByIDRepository<TEntity>,
    IRetrieveListByIDsRepository<TEntity>,
    IRetrieveAllRepository<TEntity>
    where TEntity : Entity
{

    public GenericEntityRepository(IDatabaseContext context)
        : base(context)
    { }

    public Task<TEntity> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        return Context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<IQueryable<TEntity>> RetrieveByListOfIDAsync(List<Guid> ids, bool tracking, CancellationToken cancellationToken)
    {
        return tracking
            ? Task.Run(() => Context.Set<TEntity>().Where(t => ids.Contains(t.Id)), cancellationToken)
            : Task.Run(() => Context.Set<TEntity>().AsNoTracking().Where(t => ids.Contains(t.Id)), cancellationToken);
    }

    public Task<List<TEntity>> RetrieveAllAsync(CancellationToken cancellationToken)
    {
        return Context.Set<TEntity>().ToListAsync(cancellationToken);
    }
}
