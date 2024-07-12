using RestaurantApi.Core.Application.ViewModels;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Services
{
    public interface IOrderService : IGenericService<OrderViewModel, Order>
    {
        public Task<Order> AddService(ListOrderViewModel vm, List<int> dishes);
        public  Task<List<OrderViewModel>> GetAllWithInclude();
        public Task<OrderViewModel> GetByIdWithInclude(int id);

        public Task UpdateService(UpdateOrderViewModel vm, int id);

    }
}
