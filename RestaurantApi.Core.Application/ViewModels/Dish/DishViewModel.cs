using RestaurantApi.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Dish
{
    public class DishViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]

        public double Price { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Category { get; set; }

        public ICollection<DishIngredient>? DishIngredients { get; set; }

        [JsonIgnore]
        public ICollection<OrderDish>? OrderDishes { get; set; }
    }
}