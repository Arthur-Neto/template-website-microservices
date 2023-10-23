using Template.Server.Domain.Core.Contracts;
using Template.Server.Infra.EF.Core.Contracts;

namespace Template.Server.Infra.EF.Core;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDatabaseContext
{
    private readonly TContext _context;
    private bool _disposed;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
