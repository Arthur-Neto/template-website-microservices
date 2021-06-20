using System;
using System.Threading.Tasks;

namespace Template.Domain
{
    public interface IUnitOfWork<TDbContext> : IDisposable
    {
        Task<int> CommitAsync();
    }
}
