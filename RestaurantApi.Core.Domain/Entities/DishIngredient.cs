using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Domain.Entities;

public class DishIngredient
{
    [JsonIgnore]
    public int? DishId { get; set; }
    [JsonIgnore]
    public int? IngredientId { get; set; }
    [JsonIgnore]
    public Dish Dish { get; set; }
    public Ingredient Ingredient { get; set; }
}