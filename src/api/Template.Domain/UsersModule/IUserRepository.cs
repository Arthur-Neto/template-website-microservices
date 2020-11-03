namespace Template.Domain.UsersModule
{
    public interface IUserRepository :
        ICreateRepository<User>,
        IDeleteByIDRepository<User, int>,
        IRetrieveOData<User>,
        IRetrieveByIDRepository<User, int>,
        IUpdateRepository<User>,
        ISingleOrDefaultRepository<User>,
        ICountRepository<User>
    { }
}
