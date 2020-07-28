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
        Task DeleteAsync(int id);
        Task Update(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public Task Update(User user)
        {
            _userRepository.Update(user);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return await _userRepository.RetrieveAllAsync();
        }

        public async Task<User> RetrieveByIDAsync(int id)
        {
            return await _userRepository.RetrieveByIDAsync(id);
        }
    }
}
