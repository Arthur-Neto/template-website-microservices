using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Application.UsersModule.Commands
{
    public class UserAuthenticateCommand : IRequest<Result<AuthenticatedUserModel>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserAuthenticateCommandMapping : Profile
    {
        public UserAuthenticateCommandMapping()
        {
            CreateMap<UserAuthenticateCommand, User>()
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password));
        }
    }

    public class UserAuthenticateCommandValidator : AbstractValidator<UserAuthenticateCommand>
    {
        public UserAuthenticateCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().Length(1, 50);
            RuleFor(x => x.Password).NotEmpty().Length(1, 50);
        }
    }

    public class UserAuthenticateCommandHandler : AppHandlerBase<IUserRepository>, IRequestHandler<UserAuthenticateCommand, Result<AuthenticatedUserModel>>
    {
        private readonly IConfiguration _appSettings;

        public UserAuthenticateCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IConfiguration appSettings,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        {
            _appSettings = appSettings;
        }

        public async Task<Result<AuthenticatedUserModel>> Handle(UserAuthenticateCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.SingleOrDefaultAsync(x => x.Username.Equals(request.Username));
            if (user == null)
            {
                return Result.Failure<AuthenticatedUserModel>(ErrorType.NotFound.ToString());
            }

            var isCorrectPassword = user.Password.Equals(request.Password);
            if (!isCorrectPassword)
            {
                return Result.Failure<AuthenticatedUserModel>(ErrorType.IncorrectUserPassword.ToString());
            }

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

            return Result.Success(_mapper.Map<AuthenticatedUserModel>(user));
        }
    }
}
