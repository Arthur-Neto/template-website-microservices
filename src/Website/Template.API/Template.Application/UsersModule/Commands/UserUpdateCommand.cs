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
    public class UserUpdateCommand : IRequest<Result<bool>>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().Length(1, 50);
        }
    }

    public class UserUpdateCommandHandler : AppHandlerBase<IUserRepository, ITenantDbContext>, IRequestHandler<UserUpdateCommand, Result<bool>>
    {
        public UserUpdateCommandHandler(
            IMapper mapper,
            IUnitOfWork<ITenantDbContext> unitOfWork,
            IUserRepository userRepository
        ) : base(userRepository, mapper, unitOfWork)
        { }

        public async Task<Result<bool>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> user = await _repository.RetrieveByIDAsync(request.ID, cancellationToken);
            if (user.HasNoValue)
            {
                return Result.Failure<bool>(ErrorType.NotFound.ToString());
            }

            var isUserDuplicating = await _repository.AnyAsync(x => x.Name.Equals(request.Name) && x.ID != request.ID, cancellationToken);
            if (isUserDuplicating)
            {
                return Result.Failure<bool>(ErrorType.Duplicating.ToString());
            }

            user.Value.Name = request.Name;

            _repository.Update(user.Value);

            return await CommitAsync() > 0
                ? true
                : Result.Failure<bool>(ErrorType.SaveChangesFailure.ToString());
        }
    }
}
