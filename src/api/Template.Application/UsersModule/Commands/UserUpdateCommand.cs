using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Application.UsersModule.Commands
{
    public class UserUpdateCommand : IRequest<Result<bool>>
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateCommandMapping : Profile
    {
        public UserUpdateCommandMapping()
        {
            CreateMap<UserUpdateCommand, User>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password));
        }
    }

    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Username).NotEmpty().Length(1, 50);
            RuleFor(x => x.Password).NotEmpty().Length(1, 50);
        }
    }

    public class UserUpdateCommandHandler : AppHandlerBase<IUserRepository>, IRequestHandler<UserUpdateCommand, Result<bool>>
    {
        public UserUpdateCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<bool>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.SingleOrDefaultAsync(x => x.ID == request.ID);
            if (user == null)
            {
                return Result.Failure<bool>(ErrorType.NotFound.ToString());
            }

            var countByUsername = await _repository.CountAsync(x => x.Username.Equals(request.Username) && x.ID != request.ID);

            var isUserDuplicating = countByUsername > 0;
            if (isUserDuplicating)
            {
                return Result.Failure<bool>(ErrorType.Duplicating.ToString());
            }

            _repository.Update(user);

            return await CommitAsync() > 0;
        }
    }
}
