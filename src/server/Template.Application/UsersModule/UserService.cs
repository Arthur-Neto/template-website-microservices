using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule
{
    public interface IUserService
    {
        Task<IEnumerable<User>> RetrieveAllAsync();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return await _userRepository.RetrieveAllAsync();
        }
    }
}
