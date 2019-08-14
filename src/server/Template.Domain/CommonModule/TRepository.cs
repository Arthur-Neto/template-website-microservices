using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Domain.CommonModule
{
    public interface TRepository<T> where T : TEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(int id);
        Task<bool> Delete(T entity);
        Task<int> Add(T entity);
        Task<bool> Update(T entity);
    }
}
