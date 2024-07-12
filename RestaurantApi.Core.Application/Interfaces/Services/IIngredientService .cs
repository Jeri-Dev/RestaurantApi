using RestaurantApi.Core.Application.ViewModels.Ingredients;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Services
{
    public interface IIngredientService : IGenericService<IngredientViewModel, Ingredient>
    {
        Task UpdateService(UpdateIngredientViewModel vm, int id);
    }
}
