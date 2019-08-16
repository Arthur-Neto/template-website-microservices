using System;
using System.Threading.Tasks;

namespace Template.Domain.CommonModule
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
