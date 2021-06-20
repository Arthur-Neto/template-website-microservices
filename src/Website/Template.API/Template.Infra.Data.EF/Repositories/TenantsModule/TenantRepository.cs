using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Template.Domain.TenantsModule;
using Template.Infra.Data.EF.Contexts;

namespace Template.Infra.Data.EF.Repositories.TenantsModule
{
    public class TenantRepository : GenericRepositoryBase<Tenant>, ITenantRepository
    {
        public TenantRepository(IMasterDbContext context)
            : base(context)
        { }

        public Task<int> CountAsync(Expression<Func<Tenant, bool>> expression, CancellationToken cancellationToken)
        {
            return GenericRepository.CountAsync(expression, cancellationToken);
        }

        public Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken)
        {
            return GenericRepository.CreateAsync(tenant, cancellationToken);
        }

        public void Delete(Tenant tenant)
        {
            GenericRepository.Delete(tenant);
        }

        public IQueryable<TDestination> RetrieveOData<TDestination>(IMapper mapper)
        {
            return GenericRepository.RetrieveOData<TDestination>(mapper);
        }

        public Task<Tenant> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return GenericRepository.RetrieveByIDAsync(id, cancellationToken);
        }

        public Task<Tenant> SingleOrDefaultAsync(Expression<Func<Tenant, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<Tenant, object>>[] includeExpression)
        {
            return GenericRepository.SingleOrDefaultAsync(expression, tracking, cancellationToken, includeExpression);
        }

        public void Update(Tenant tenant)
        {
            GenericRepository.Update(tenant);
        }

        public Task<bool> AnyAsync(Expression<Func<Tenant, bool>> expression, CancellationToken cancellationToken)
        {
            return GenericRepository.AnyAsync(expression, cancellationToken);
        }
    }
}
