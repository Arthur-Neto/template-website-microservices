using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Domain.CommonModule;
using Template.Infra.Data.EF.Context;

namespace Template.Infra.Data.EF.Repositories
{
    public abstract class GenericRepository<T> :
        GetRepository<T>,
        AddRepository<T>,
        RemoveRepository<T>,
        UpdateRepository<T>
        where T : class
    {
        private readonly DbSet<T> _entities;

        public GenericRepository(ExampleContext context)
        {
            _entities = context.Set<T>();
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}
