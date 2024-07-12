using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Repositories
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        public Task<Dish> Add(Dish entity, List<Ingredient> ingredientes);
        public Task<List<Dish>> GetAllWithInclude();
        public Task<Dish> GetByIdWithInclude(int id);

        public Task Update(Dish entity, int id, List<Ingredient> dishIngredients);

    }
}
