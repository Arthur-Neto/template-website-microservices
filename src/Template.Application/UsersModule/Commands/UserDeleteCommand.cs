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
    public class UserDeleteCommand : IRequest<Result<bool>>
    {
        public int ID { get; set; }
    }

    public class UserDeleteCommandMapping : Profile
    {
        public UserDeleteCommandMapping()
        {
            CreateMap<UserDeleteCommand, User>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID));
        }
    }

    public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
    {
        public UserDeleteCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
        }
    }

    public class UserDeleteCommandHandler : AppHandlerBase<IUserRepository>, IRequestHandler<UserDeleteCommand, Result<bool>>
    {
        public UserDeleteCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<bool>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.RetrieveByIDAsync(request.ID, cancellationToken);
            if (user == null)
            {
                return Result.Failure<bool>(ErrorType.NotFound.ToString());
            }

            _repository.Delete(user);

            return await CommitAsync() > 0;
        }
    }
}
