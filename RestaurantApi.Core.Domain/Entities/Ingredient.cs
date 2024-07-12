using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //Navigation Properties
        [JsonIgnore]

        public ICollection<DishIngredient>? DishIngredients { get; set; }

    }
}
