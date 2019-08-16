using System.Threading.Tasks;
using Template.Domain.CommonModule;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExampleContext _context;

        public UnitOfWork(ExampleContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
