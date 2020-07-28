using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule
{
    public interface IUserService
    {
        Task<IEnumerable<User>> RetrieveAllAsync();
        Task<User> RetrieveByIDAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> Update(User user);
    }

    public class UserService : AppServiceBase<IUserRepository>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IUserRepository repository)
            : base(unitOfWork, repository)
        { }

        public async Task<User> CreateAsync(User user)
        {
            var createdUser = await _repository.CreateAsync(user);

            return await CommitAsync() > 0 ? createdUser : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);

            return await CommitAsync() > 0;
        }

        public async Task<bool> Update(User user)
        {
            _repository.Update(user);

            return await CommitAsync() > 0;
        }

        public async Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return await _repository.RetrieveAllAsync();
        }

        public async Task<User> RetrieveByIDAsync(int id)
        {
            return await _repository.RetrieveByIDAsync(id);
        }
    }
}
