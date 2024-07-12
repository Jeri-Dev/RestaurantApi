using RestaurantApi.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Orders
{
    public class ListOrderViewModel
    {
        public int Id { get; set; }

        [JsonIgnore]

        public double Subtotal { get; set; }
        [JsonIgnore]

        public string State { get; set; } = "En Proceso";
        public int TableId { get; set; }
        public List<int> DishesIds { get; set; }

        [JsonIgnore]
        public ICollection<OrderDish>? OrderDishes { get; set; }
    }
}
