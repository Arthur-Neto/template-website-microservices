using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Template.Domain.UsersModule;
using Template.Domain.UsersModule.Enums;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Application.UsersModule.Commands
{
    public class UserCreateCommand : IRequest<Result<int>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserCreateCommandMapping : Profile
    {
        public UserCreateCommandMapping()
        {
            CreateMap<UserCreateCommand, User>()
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password));
        }
    }

    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().Length(1, 50);
            RuleFor(x => x.Password).NotEmpty().Length(1, 50);
        }
    }

    public class UserCreateCommandHandler : AppHandlerBase<IUserRepository>, IRequestHandler<UserCreateCommand, Result<int>>
    {
        public UserCreateCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<int>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var countByUsername = await _repository.CountAsync(x => x.Username.Equals(request.Username));

            var isUserDuplicating = countByUsername > 0;
            if (isUserDuplicating)
            {
                return Result.Failure<int>(ErrorType.Duplicating.ToString());
            }

            var user = _mapper.Map<User>(request);
            user.Role = Role.Client;

            var createdUser = await _repository.CreateAsync(user);

            return await CommitAsync() > 0 ? createdUser.ID : 0;
        }
    }
}
