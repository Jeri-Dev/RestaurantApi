using RestaurantApi.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Orders
{
    public class UpdateOrderViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public double Subtotal { get; set; }
        public string State { get; set; } = "En Proceso";
        public int TableId { get; set; }
        public List<int> DishesIds { get; set; }

        [JsonIgnore]
        public ICollection<OrderDish>? OrderDishes { get; set; }
    }
}
