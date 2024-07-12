using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApi.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
    where T : class
    {
        private readonly ApplicationContext _dbContext;

        public GenericRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<T> Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Update(T entity, int id)
        {
            var entry = await _dbContext.Set<T>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
    }
}
