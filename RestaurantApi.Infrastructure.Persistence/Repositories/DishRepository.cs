using Microsoft.EntityFrameworkCore;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Contexts;
using System.Linq;

namespace RestaurantApi.Infrastructure.Persistence.Repositories
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {

        private readonly ApplicationContext _dbContext;
        private readonly IIngredientRepository _ingredientRepository;

        public DishRepository(ApplicationContext context, IIngredientRepository ingredientRepository) : base(context)
        {
            _dbContext = context;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<Dish> Add(Dish entity, List<Ingredient> ingredientes)
        {
            foreach (Ingredient item in ingredientes)
            {
                var ingredient = await _ingredientRepository.GetById(item.Id);

                if (item != null && item.Id != 0 && ingredient != null) {
                    DishIngredient dishIngredient = new DishIngredient()
                    {
                        Ingredient = item,
                        IngredientId = item.Id,
                        Dish = entity,
                        DishId = entity.Id
                    };
                    await _dbContext.Set<DishIngredient>().AddAsync(dishIngredient);
                }
            }

            await _dbContext.Set<Dish>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(Dish entity, int id, List<Ingredient> dishIngredients)
        {
            // Obtener el plato actual de la base de datos junto con sus ingredientes asociados
            var existingDish = await _dbContext.Set<Dish>()
                                               .Include(d => d.DishIngredients)
                                               .ThenInclude(di => di.Ingredient)
                                               .FirstOrDefaultAsync(d => d.Id == id);

            if (existingDish == null)
            {
                throw new ArgumentException("Dish not found.");
            }

            // Obtener la lista de ingredientes actuales asociados al plato
            var currentIngredientIds = existingDish.DishIngredients.Select(di => di.IngredientId).ToList();
            var newIngredientIds = dishIngredients.Select(i => i.Id).ToList();

            // Eliminar ingredientes que ya no están asociados
            var ingredientsToRemove = existingDish.DishIngredients
                                                  .Where(di => !newIngredientIds.Contains((int)di.IngredientId))
                                                  .ToList();

            foreach (var dishIngredient in ingredientsToRemove)
            {
                _dbContext.Set<DishIngredient>().Remove(dishIngredient);
            }

            // Agregar nuevos ingredientes que no están actualmente asociados
            var ingredientsToAdd = dishIngredients
                                   .Where(i => !currentIngredientIds.Contains(i.Id))
                                   .ToList();

            foreach (var ingredient in ingredientsToAdd)
            {
                var ingredientFromDb = await _ingredientRepository.GetById(ingredient.Id);

                if (ingredientFromDb != null)
                {
                    DishIngredient dishIngredient = new DishIngredient()
                    {
                        Ingredient = ingredientFromDb,
                        IngredientId = ingredientFromDb.Id,
                        Dish = existingDish,
                        DishId = existingDish.Id
                    };
                    await _dbContext.Set<DishIngredient>().AddAsync(dishIngredient);
                }
            }

            // Actualizar los campos del plato
            _dbContext.Entry(existingDish).CurrentValues.SetValues(entity);

            // Guardar los cambios
            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<Dish>> GetAllWithInclude()
        {
            var dbSet = _dbContext.Set<Dish>()
                .Include(dish => dish.DishIngredients)
                .ThenInclude(dishIngredient => dishIngredient.Ingredient)
                .Include(dish => dish.OrderDishes)
                .ThenInclude(orderDish => orderDish.Order);

            return await dbSet.ToListAsync();
        }

        public async Task<Dish> GetByIdWithInclude(int id)
        {
            var list = await GetAllWithInclude();
            var dish = list.FirstOrDefault(dish => dish.Id == id);

            return dish;
        }

        
    }
}
