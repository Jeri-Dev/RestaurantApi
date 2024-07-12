using RestaurantApi.Core.Application.ViewModels.Dish;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Services
{
    public interface IDishService : IGenericService<DishViewModel, Dish>
    {
        public Task<Dish> AddService(ListDishViewModel entity, List<int> ingredientes);
        public Task<List<DishViewModel>> GetAllWithInclude();
        public Task<DishViewModel> GetByIdWithInclude(int id);
        public Task UpdateService(UpdateDishViewModel vm, int id);



    }
}
