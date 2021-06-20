namespace Template.Domain.UsersModule
{
    public interface IUserRepository :
        ICreateRepository<User>,
        IDeleteRepository<User>,
        IRetrieveOData,
        IRetrieveByIDRepository<User>,
        IUpdateRepository<User>,
        ISingleOrDefaultRepository<User>,
        ICountRepository<User>,
        IAnyRepository<User>
    { }
}
