using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Queries
{
    public class UserRetrieveByIDQuery : IRequest<Result<UserModel>>
    {
        public int ID { get; set; }
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
            return _mapper.Map<UserModel>(await _repository.RetrieveByIDAsync(query.ID));
        }
    }
}
