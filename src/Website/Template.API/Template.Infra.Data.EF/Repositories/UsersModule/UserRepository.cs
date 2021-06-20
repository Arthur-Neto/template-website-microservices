using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Contexts;

namespace Template.Infra.Data.EF.Repositories.UsersModule
{
    public class UserRepository : GenericRepositoryBase<User>, IUserRepository
    {
        public UserRepository(ITenantDbContext context)
            : base(context)
        { }

        public Task<int> CountAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken)
        {
            return GenericRepository.CountAsync(expression, cancellationToken);
        }

        public Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            return GenericRepository.CreateAsync(user, cancellationToken);
        }

        public void Delete(User user)
        {
            GenericRepository.Delete(user);
        }

        public IQueryable<TDestination> RetrieveOData<TDestination>(IMapper mapper)
        {
            return GenericRepository.RetrieveOData<TDestination>(mapper);
        }

        public Task<User> RetrieveByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return GenericRepository.RetrieveByIDAsync(id, cancellationToken);
        }

        public Task<User> SingleOrDefaultAsync(Expression<Func<User, bool>> expression, bool tracking, CancellationToken cancellationToken, params Expression<Func<User, object>>[] includeExpression)
        {
            return GenericRepository.SingleOrDefaultAsync(expression, tracking, cancellationToken, includeExpression);
        }

        public void Update(User user)
        {
            GenericRepository.Update(user);
        }

        public Task<bool> AnyAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken)
        {
            return GenericRepository.AnyAsync(expression, cancellationToken);
        }
    }
}
