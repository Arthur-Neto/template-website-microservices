namespace Template.Server.Domain.Core.Contracts;

public interface IUnitOfWork<TDbContext> : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
