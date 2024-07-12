using Microsoft.EntityFrameworkCore;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Contexts;

namespace RestaurantApi.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly IDishRepository _dishRepository;
        private readonly ApplicationContext _dbContext;

        public OrderRepository(ApplicationContext context, IDishRepository dishRepository) : base(context)
        {
            _dishRepository = dishRepository;
            _dbContext = context;
        }

        public async Task<Order> Add(Order entity, List<Dish> dishes)
        {
            foreach (Dish item in dishes)
            {
                var dish = await _dishRepository.GetById(item.Id);

                if (item != null && item.Id != 0 && dish != null)
                {
                    OrderDish orderDish = new OrderDish()
                    {
                        Dish = item,
                        DishId = item.Id,
                        Order = entity,
                        OrderId = entity.Id
                    };
                    await _dbContext.Set<OrderDish>().AddAsync(orderDish);
                }
            }

            await _dbContext.Set<Order>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<List<Order>> GetAllWithInclude()
        {
            var dbSet = _dbContext.Set<Order>()
                .Include(dish => dish.OrderDishes)
                .ThenInclude(OrderDish => OrderDish.Dish);

            return await dbSet.ToListAsync();
        }

        public async Task<Order> GetByIdWithInclude(int id)
        {
            var list = await GetAllWithInclude();
            var order = list.FirstOrDefault(order => order.Id == id);

            return order;
        }

        public async Task Update(Order entity, int id, List<Dish> orderDishes)
        {
            // Obtener el plato actual de la base de datos junto con sus ingredientes asociados
            var existingOrder = await _dbContext.Set<Order>()
                                               .Include(d => d.OrderDishes)
                                               .ThenInclude(di => di.Dish)
                                               .FirstOrDefaultAsync(d => d.Id == id);

            if (existingOrder == null)
            {
                throw new ArgumentException("Order not found.");
            }

            // Obtener la lista de ingredientes actuales asociados al plato
            var currentDishIds = existingOrder.OrderDishes.Select(di => di.DishId).ToList();
            var newDishIds = orderDishes.Select(i => i.Id).ToList();

            // Eliminar ingredientes que ya no están asociados
            var dishesToRemove = existingOrder.OrderDishes
                                                  .Where(di => !newDishIds.Contains((int)di.DishId))
                                                  .ToList();

            foreach (var orderDish in dishesToRemove)
            {
                _dbContext.Set<OrderDish>().Remove(orderDish);
            }

            // Agregar nuevos ingredientes que no están actualmente asociados
            var dishesToAdd = orderDishes
                                   .Where(i => !currentDishIds.Contains(i.Id))
                                   .ToList();

            foreach (var dish in dishesToAdd)
            {
                var dishFromDb = await _dishRepository.GetById(dish.Id);

                if (dishFromDb != null)
                {
                    OrderDish orderDish = new OrderDish()
                    {
                        Dish = dishFromDb,
                        DishId = dishFromDb.Id,
                        Order = existingOrder,
                        OrderId = existingOrder.Id
                    };
                    await _dbContext.Set<OrderDish>().AddAsync(orderDish);
                }
            }

            // Actualizar los campos del plato
            _dbContext.Entry(existingOrder).CurrentValues.SetValues(entity);

            // Guardar los cambios
            await _dbContext.SaveChangesAsync();
        }
    }
}
