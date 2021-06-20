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
    public class UserCreateCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
    }

    public class UserCreateCommandMapping : Profile
    {
        public UserCreateCommandMapping()
        {
            CreateMap<UserCreateCommand, User>()
                .ForMember(m => m.Name, opts => opts.MapFrom(src => src.Name));
        }
    }

    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50);
        }
    }

    public class UserCreateCommandHandler : AppHandlerBase<IUserRepository, ITenantDbContext>, IRequestHandler<UserCreateCommand, Result<Guid>>
    {
        public UserCreateCommandHandler(
            IMapper mapper,
            IUnitOfWork<ITenantDbContext> unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<Guid>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var isUserDuplicating = await _repository.AnyAsync(x => x.Name.Equals(request.Name), cancellationToken);
            if (isUserDuplicating)
            {
                return Result.Failure<Guid>(ErrorType.Duplicating.ToString());
            }

            var user = _mapper.Map<User>(request);

            var createdUser = await _repository.CreateAsync(user, cancellationToken);

            return await CommitAsync() > 0
                ? createdUser.ID
                : Result.Failure<Guid>(ErrorType.SaveChangesFailure.ToString());
        }
    }
}
