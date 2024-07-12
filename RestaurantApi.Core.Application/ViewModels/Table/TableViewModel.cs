using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Table;

    public class TableViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }

        [JsonIgnore]
        public string State { get; set; } = "Disponible";
    }

