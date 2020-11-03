using System;
using System.Threading.Tasks;

namespace Template.Application
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
