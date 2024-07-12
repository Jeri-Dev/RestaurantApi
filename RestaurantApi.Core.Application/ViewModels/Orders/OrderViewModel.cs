using RestaurantApi.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public double Subtotal { get; set; }

        public string State { get; set; } = "En Proceso";

        [JsonIgnore]
        public List<int> DishesIds { get; set; }
        public int TableId { get; set; }


        public ICollection<OrderDish>? OrderDishes { get; set; }
    }
}
