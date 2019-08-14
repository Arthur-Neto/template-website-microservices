namespace Template.Application
{
    public class BaseAppService<TRepository>
    {
        protected TRepository Repository;

        public BaseAppService(TRepository repository)
        {
            Repository = repository;
        }
    }
}
