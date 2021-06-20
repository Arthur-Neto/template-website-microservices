using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Application.UsersModule.Queries
{
    public class UserRetrieveByIDQuery : IRequest<Result<UserModel>>
    {
        public Guid ID { get; set; }
    }

    public class UserRetrieveByIDQueryHandler : AppHandlerBase<IUserRepository>, IRequestHandler<UserRetrieveByIDQuery, Result<UserModel>>
    {
        public UserRetrieveByIDQueryHandler(
            IMapper mapper,
            IUserRepository userRepository
        ) : base(userRepository, mapper)
        { }

        public async Task<Result<UserModel>> Handle(UserRetrieveByIDQuery query, CancellationToken cancellationToken)
        {
            Maybe<User> user = await _repository.RetrieveByIDAsync(query.ID, cancellationToken);
            if (user.HasNoValue)
            {
                return Result.Failure<UserModel>(ErrorType.NotFound.ToString());
            }

            return Result.Success(_mapper.Map<UserModel>(user.Value));
        }
    }
}
