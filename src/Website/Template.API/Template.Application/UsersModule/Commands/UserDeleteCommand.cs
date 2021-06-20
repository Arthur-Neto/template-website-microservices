using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Template.Domain;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;
using Template.Infra.Data.EF.Contexts;

namespace Template.Application.UsersModule.Commands
{
    public class UserDeleteCommand : IRequest<Result<bool>>
    {
        public Guid ID { get; set; }
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
            RuleFor(x => x.ID).NotEmpty();
        }
    }

    public class UserDeleteCommandHandler : AppHandlerBase<IUserRepository, ITenantDbContext>, IRequestHandler<UserDeleteCommand, Result<bool>>
    {
        public UserDeleteCommandHandler(
            IMapper mapper,
            IUnitOfWork<ITenantDbContext> unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<bool>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> user = await _repository.RetrieveByIDAsync(request.ID, cancellationToken);
            if (user.HasNoValue)
            {
                return Result.Failure<bool>(ErrorType.NotFound.ToString());
            }

            _repository.Delete(user.Value);

            return await CommitAsync() > 0
                ? true
                : Result.Failure<bool>(ErrorType.SaveChangesFailure.ToString());
        }
    }
}
