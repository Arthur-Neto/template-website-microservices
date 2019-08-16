namespace Template.Application
{
    public abstract class BaseAppService<TRepository>
    {
        protected TRepository Repository;

        public BaseAppService(TRepository repository)
        {
            Repository = repository;
        }
    }
}
