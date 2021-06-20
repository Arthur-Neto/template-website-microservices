using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Template.Domain.EnterprisesModule;
using Template.Infra.Data.EF.Contexts;

namespace Template.Infra.Data.EF.Repositories.EnterprisesModule
{
    public class EnterpriseRepository : GenericRepositoryBase<Enterprise>, IEnterpriseRepository
    {
        public EnterpriseRepository(IMasterDbContext context)
            : base(context)
        { }

        public Task<Enterprise> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return GenericRepository.RetrieveByIDAsync(id, cancellationToken);
        }

        public Task<Enterprise> SingleOrDefaultAsync(Expression<Func<Enterprise, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<Enterprise, object>>[] includeExpression)
        {
            return GenericRepository.SingleOrDefaultAsync(expression, tracking, cancellationToken, includeExpression);
        }
    }
}
