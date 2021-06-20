using System.Threading.Tasks;
using Template.Domain;
using Template.Infra.Data.EF.Contexts;

namespace Template.Infra.Data.EF.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDatabaseContext
    {
        private readonly TContext _context;
        private bool _disposed;

        public UnitOfWork(TContext context)
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
