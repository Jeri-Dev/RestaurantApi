using RestaurantApi.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Dish
{
    public class ListDishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
        public string Category { get; set; }

        public List<int> IngredientsIds { get; set; }

        [JsonIgnore]
        public ICollection<DishIngredient>? DishIngredients { get; set; }

        [JsonIgnore]
        public ICollection<OrderDish>? OrderDishes { get; set; }
    }
}