using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Template.Domain.EnterprisesModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Application.EnterprisesModule.Queries
{
    public class RetrieveConnectionStringByEnterpriseNameQuery : IRequest<Result<string>>
    {
        public string EnterpriseName { get; set; }
    }

    public class RetrieveConnectionStringByEnterpriseNameQueryHandler : AppHandlerBase<IEnterpriseRepository>, IRequestHandler<RetrieveConnectionStringByEnterpriseNameQuery, Result<string>>
    {
        public RetrieveConnectionStringByEnterpriseNameQueryHandler(
            IEnterpriseRepository userRepository
        ) : base(userRepository)
        { }

        public async Task<Result<string>> Handle(RetrieveConnectionStringByEnterpriseNameQuery query, CancellationToken cancellationToken)
        {
            Maybe<Enterprise> enterprise = await _repository.SingleOrDefaultAsync(p => p.NormalizedEnterpriseName.Equals(query.EnterpriseName), tracking: false, cancellationToken);
            if (enterprise.HasNoValue)
            {
                return Result.Failure<string>(ErrorType.EnterpriseNotFound.ToString());
            }

            return Result.Success(enterprise.Value.ConnectionString);
        }
    }
}
