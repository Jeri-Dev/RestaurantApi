namespace RestaurantApi.Core.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Subtotal { get; set; }
        public string State { get; set; }


        public int TableId { get; set; }

        //Navigation Properties
        public Table Table { get; set; }
        public ICollection<OrderDish>? OrderDishes { get; set; }

    }
}
