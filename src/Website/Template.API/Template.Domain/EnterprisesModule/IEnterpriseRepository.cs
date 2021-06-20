namespace Template.Domain.EnterprisesModule
{
    public interface IEnterpriseRepository :
        IRetrieveByIDRepository<Enterprise>,
        ISingleOrDefaultRepository<Enterprise>
    { }
}
