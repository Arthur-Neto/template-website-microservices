using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Template.Application.TenantsModule.Models;
using Template.Domain.TenantsModule;
using Template.Infra.Crosscutting.Exceptions;
using Template.Security;

namespace Template.Application.TenantsModule.Commands
{
    public class TenantAuthenticateCommand : IRequest<Result<AuthenticatedTenantModel>>
    {
        public string Logon { get; set; }
        public string Password { get; set; }
    }

    public class TenantAuthenticateCommandMapping : Profile
    {
        public TenantAuthenticateCommandMapping()
        {
            CreateMap<TenantAuthenticateCommand, Tenant>()
                .ForMember(m => m.Logon, opts => opts.MapFrom(src => src.Logon));
        }
    }

    public class TenantAuthenticateCommandValidator : AbstractValidator<TenantAuthenticateCommand>
    {
        public TenantAuthenticateCommandValidator()
        {
            RuleFor(x => x.Logon).NotEmpty().Length(1, 50);
            RuleFor(x => x.Password).NotEmpty().Length(1, 50);
        }
    }

    public class TenantAuthenticateCommandHandler : AppHandlerBase<ITenantRepository>, IRequestHandler<TenantAuthenticateCommand, Result<AuthenticatedTenantModel>>
    {
        private readonly IConfiguration _appSettings;
        private readonly IHashing _hashing;
        private readonly IJwtTokenFactory _jwtTokenFactory;

        public TenantAuthenticateCommandHandler(
            IMapper mapper,
            ILogger<TenantAuthenticateCommandHandler> logger,
            IConfiguration appSettings,
            ITenantRepository tenantRepository,
            IHashing hashing,
            IJwtTokenFactory jwtTokenFactory
        ) : base(tenantRepository, mapper, logger)
        {
            _appSettings = appSettings;
            _hashing = hashing;
            _jwtTokenFactory = jwtTokenFactory;
        }

        public async Task<Result<AuthenticatedTenantModel>> Handle(TenantAuthenticateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("New request with {logon} and {password}", request.Logon, request.Password);

            var secret = _appSettings.GetValue<string>("Secret");
            if (secret.Length < 15)
            {
                return Result.Failure<AuthenticatedTenantModel>(ErrorType.SecretKeyTooShort.ToString());
            }

            Maybe<Tenant> tenant = await _repository.SingleOrDefaultAsync(x => x.Logon.Equals(request.Logon), tracking: true, cancellationToken, x => x.Enterprise);
            if (tenant.HasNoValue)
            {
                return Result.Failure<AuthenticatedTenantModel>(ErrorType.NotFound.ToString());
            }

            var correctPassword = _hashing.IsValidHash(tenant.Value.Password, tenant.Value.Salt, request.Password);
            if (correctPassword is false)
            {
                return Result.Failure<AuthenticatedTenantModel>(ErrorType.IncorrectUserPassword.ToString());
            }

            var parsedExpiration = double.TryParse(_appSettings.GetValue<string>("TokenExpiration"), out var tokenExpiration);
            if (parsedExpiration is false)
            {
                tokenExpiration = 60;
            }

            tenant.Value.Token = _jwtTokenFactory.CreateToken(secret, tokenExpiration, tenant.Value.ID.ToString(), tenant.Value.Role.ToString(), tenant.Value.Enterprise.NormalizedEnterpriseName);

            return Result.Success(_mapper.Map<AuthenticatedTenantModel>(tenant.Value));
        }
    }
}
