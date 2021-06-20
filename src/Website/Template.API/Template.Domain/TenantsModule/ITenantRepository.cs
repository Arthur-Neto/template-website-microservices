namespace Template.Domain.TenantsModule
{
    public interface ITenantRepository :
        ICreateRepository<Tenant>,
        IDeleteRepository<Tenant>,
        IRetrieveOData,
        IRetrieveByIDRepository<Tenant>,
        IUpdateRepository<Tenant>,
        ISingleOrDefaultRepository<Tenant>,
        ICountRepository<Tenant>,
        IAnyRepository<Tenant>
    { }
}
