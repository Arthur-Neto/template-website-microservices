using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Domain.CommonModule
{
    public interface AddRepository<T> where T : class
    {
        Task AddAsync(T entity);
    }

    public interface GetRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIDAsync(int id);
    }

    public interface RemoveRepository<T> where T : class
    {
        void Remove(T entity);
    }

    public interface UpdateRepository<T> where T : class
    {
        void Update(T entity);
    }
}
