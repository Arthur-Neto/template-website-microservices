using System.Collections.Generic;
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

        public Task<User> CreateAsync(User user)
        {
            return GenericRepository.CreateAsync(user);
        }

        public Task DeleteAsync(int id)
        {
            return GenericRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return GenericRepository.RetrieveAllAsync();
        }

        public Task<User> RetrieveByIDAsync(int id)
        {
            return GenericRepository.RetrieveByIDAsync(id);
        }

        public void Update(User user)
        {
            GenericRepository.Update(user);
        }
    }
}
