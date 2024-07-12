using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Domain.Entities;

public class OrderDish
{
    public int? DishId { get; set; }

    [JsonIgnore]

    public int? OrderId { get; set; }
    public Dish? Dish { get; set; }

    [JsonIgnore]

    public Order? Order { get; set; }
}