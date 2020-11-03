using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Template.Domain.UsersModule;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.Repositories.UsersModule
{
    public class UserRepository : GenericRepositoryBase<User, int>, IUserRepository
    {
        public UserRepository(IDatabaseContext context)
            : base(context)
        { }

        public Task<int> CountAsync(Expression<Func<User, bool>> expression)
        {
            return GenericRepository.CountAsync(expression);
        }

        public Task<User> CreateAsync(User user)
        {
            return GenericRepository.CreateAsync(user);
        }

        public Task DeleteAsync(int id)
        {
            return GenericRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<User>> RetrieveAllAsync(Expression<Func<User, bool>> expression = null, params Expression<Func<User, object>>[] includeExpression)
        {
            return GenericRepository.RetrieveAllAsync(expression, includeExpression);
        }

        public Task<User> RetrieveByIDAsync(int id)
        {
            return GenericRepository.RetrieveByIDAsync(id);
        }

        public Task<User> SingleOrDefaultAsync(Expression<Func<User, bool>> expression, bool tracking = true, params Expression<Func<User, object>>[] includeExpression)
        {
            return GenericRepository.SingleOrDefaultAsync(expression, tracking, includeExpression);
        }

        public void Update(User user)
        {
            GenericRepository.Update(user);
        }
    }
}
