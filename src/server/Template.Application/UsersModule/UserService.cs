using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> RetrieveAllAsync();
        Task<UserModel> RetrieveByIDAsync(int id);
        Task<int> CreateAsync(UserCreateCommand command);
        Task<bool> DeleteAsync(UserDeleteCommand command);
        Task<bool> Update(UserUpdateCommand command);
    }

    public class UserService : AppServiceBase<IUserRepository>, IUserService
    {
        public UserService(IUserRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(repository, mapper, unitOfWork)
        { }

        public async Task<int> CreateAsync(UserCreateCommand command)
        {
            var user = _mapper.Map<User>(command);
            var createdUser = await _repository.CreateAsync(user);

            return await CommitAsync() > 0 ? createdUser.ID : 0;
        }

        public async Task<bool> DeleteAsync(UserDeleteCommand command)
        {
            var user = _mapper.Map<User>(command);
            await _repository.DeleteAsync(user.ID);

            return await CommitAsync() > 0;
        }

        public async Task<bool> Update(UserUpdateCommand command)
        {
            var user = _mapper.Map<User>(command);
            _repository.Update(user);

            return await CommitAsync() > 0;
        }

        public async Task<IEnumerable<UserModel>> RetrieveAllAsync()
        {
            var users = await _repository.RetrieveAllAsync();

            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> RetrieveByIDAsync(int id)
        {
            var user = await _repository.RetrieveByIDAsync(id);

            return _mapper.Map<UserModel>(user);
        }
    }
}
