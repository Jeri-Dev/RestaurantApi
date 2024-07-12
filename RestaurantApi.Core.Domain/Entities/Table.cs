namespace RestaurantApi.Core.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string State { get; set; } = "Disponible";


        //Navigation Properties
        public ICollection<Order> Orders { get; set; }
    }

}
