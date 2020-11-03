using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;
using Template.Domain.UsersModule.Enums;
using Template.Infra.Crosscutting.Exceptions;
using Template.Infra.Crosscutting.Guards;

namespace Template.Application.UsersModule
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> RetrieveAllAsync();
        Task<UserModel> RetrieveByIDAsync(int id);
        Task<int> CreateAsync(UserCreateCommand command);
        Task<bool> DeleteAsync(UserDeleteCommand command);
        Task<bool> UpdateAsync(UserUpdateCommand command);

        Task<AuthenticatedUserModel> AuthenticateAsync(UserAuthenticateCommand command);
    }

    public class UserService : AppServiceBase<IUserRepository>, IUserService
    {
        private readonly IConfiguration _appSettings;

        public UserService(
            IConfiguration appSettings,
            IUserRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(repository, mapper, unitOfWork)
        {
            _appSettings = appSettings;
        }

        public async Task<int> CreateAsync(UserCreateCommand command)
        {
            var userCountByUsername = await _repository.CountAsync(x => x.Username.Equals(command.Username));
            Guard.Against(userCountByUsername > 0, ErrorType.Duplicating);

            var user = _mapper.Map<User>(command);
            user.Role = Role.Client;

            var createdUser = await _repository.CreateAsync(user);

            return await CommitAsync() > 0 ? createdUser.ID : 0;
        }

        public async Task<bool> DeleteAsync(UserDeleteCommand command)
        {
            var user = _mapper.Map<User>(command);
            await _repository.DeleteAsync(user.ID);

            return await CommitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UserUpdateCommand command)
        {
            var user = await _repository.SingleOrDefaultAsync(x => x.ID == command.ID, tracking: false);
            Guard.Against(user, ErrorType.NotFound);

            var userCountByUsername = await _repository.CountAsync(x => x.Username.Equals(command.Username) && x.ID != command.ID);
            Guard.Against(userCountByUsername > 0, ErrorType.Duplicating);

            user = _mapper.Map<User>(command);

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

        public async Task<AuthenticatedUserModel> AuthenticateAsync(UserAuthenticateCommand command)
        {
            var user = await _repository.SingleOrDefaultAsync(x => x.Username.Equals(command.Username));
            Guard.Against(user, ErrorType.NotFound);

            var isCorrectPassword = user.Password.Equals(command.Password);
            Guard.Against(!isCorrectPassword, ErrorType.IncorrectUserPassword);

            var tokenExpiration = _appSettings.GetValue<string>("TokenExpiration");
            var secret = _appSettings.GetValue<string>("Secret");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            await CommitAsync();

            return _mapper.Map<AuthenticatedUserModel>(user);
        }
    }
}
