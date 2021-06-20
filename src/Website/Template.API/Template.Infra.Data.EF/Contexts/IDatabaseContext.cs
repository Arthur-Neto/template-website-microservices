using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Template.Infra.Data.EF.Contexts
{
    public interface IDatabaseContext : IDisposable
    {
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IMasterDbContext : IDatabaseContext
    { }

    public interface ITenantDbContext : IDatabaseContext
    { }
}
