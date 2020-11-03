using System.Threading.Tasks;
using Template.Application;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext _context;
        private bool _disposed;

        public UnitOfWork(IDatabaseContext context)
        {
            _context = context;
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
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
        }
    }
}
