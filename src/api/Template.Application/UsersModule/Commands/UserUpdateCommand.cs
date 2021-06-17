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
            Maybe<User> user = await _repository.RetrieveByIDAsync(request.ID, cancellationToken);
            if (user.HasNoValue)
            {
                return Result.Failure<bool>(ErrorType.NotFound.ToString());
            }

            var isUserDuplicating = await _repository.AnyAsync(x => x.Username.Equals(request.Username) && x.ID != request.ID, cancellationToken);
            if (isUserDuplicating)
            {
                return Result.Failure<bool>(ErrorType.Duplicating.ToString());
            }

            user.Value.Username = request.Username;
            user.Value.Password = request.Password;

            _repository.Update(user.Value);

            return await CommitAsync() > 0;
        }
    }
}
