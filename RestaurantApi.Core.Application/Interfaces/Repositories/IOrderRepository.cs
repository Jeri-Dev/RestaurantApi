using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> Add(Order entity, List<Dish> dishes);

        public Task<List<Order>> GetAllWithInclude();
        public Task<Order> GetByIdWithInclude(int id);
        public Task Update(Order entity, int id, List<Dish> orderDishes);



    }
}
