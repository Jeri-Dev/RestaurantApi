using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Domain.Entities
{

    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
        public string Category {  get; set; }


        //Navigation Properties
        public ICollection<DishIngredient>? DishIngredients { get; set; }

        [JsonIgnore]
        public ICollection<OrderDish>? OrderDishes { get; set; }


    }
}
