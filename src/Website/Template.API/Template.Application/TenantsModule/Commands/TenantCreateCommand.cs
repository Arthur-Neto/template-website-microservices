using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Template.Domain;
using Template.Domain.TenantsModule;
using Template.Domain.TenantsModule.Enums;
using Template.Infra.Crosscutting.Exceptions;
using Template.Infra.Data.EF.Contexts;
using Template.Security;

namespace Template.Application.TenantsModule.Commands
{
    public class TenantCreateCommand : IRequest<Result<Guid>>
    {
        public string Logon { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public class TenantCreateCommandMapping : Profile
    {
        public TenantCreateCommandMapping()
        {
            CreateMap<TenantCreateCommand, Tenant>()
                .ForMember(m => m.Logon, opts => opts.MapFrom(src => src.Logon))
                .ForMember(m => m.Role, opts => opts.MapFrom(src => src.Role));
        }
    }

    public class TenantCreateCommandValidator : AbstractValidator<TenantCreateCommand>
    {
        public TenantCreateCommandValidator()
        {
            RuleFor(x => x.Logon).NotEmpty().Length(1, 50);
        }
    }

    public class TenantCreateCommandHandler : AppHandlerBase<ITenantRepository, ITenantDbContext>, IRequestHandler<TenantCreateCommand, Result<Guid>>
    {
        private readonly IHashing _hashing;

        public TenantCreateCommandHandler(
            IMapper mapper,
            IUnitOfWork<ITenantDbContext> unitOfWork,
            ITenantRepository tenantRepository,
            IHashing hashing
        ) : base(tenantRepository, mapper, unitOfWork)
        {
            _hashing = hashing;
        }

        public async Task<Result<Guid>> Handle(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            var isTenantDuplicating = await _repository.AnyAsync(x => x.Logon.Equals(request.Logon), cancellationToken);
            if (isTenantDuplicating)
            {
                return Result.Failure<Guid>(ErrorType.Duplicating.ToString());
            }

            var tenant = _mapper.Map<Tenant>(request);
            tenant.Salt = _hashing.GenerateSalt();
            tenant.Password = _hashing.GenerateHash(request.Password, tenant.Salt);

            var createdTenant = await _repository.CreateAsync(tenant, cancellationToken);

            return await CommitAsync() > 0
                ? createdTenant.ID
                : Result.Failure<Guid>(ErrorType.SaveChangesFailure.ToString());
        }
    }
}
