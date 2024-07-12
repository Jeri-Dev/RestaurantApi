using System.Text.Json.Serialization;

namespace RestaurantApi.Core.Application.ViewModels.Table;

    public class ChangeStatusTableViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        [JsonIgnore]
        public int Capacity { get; set; }

        public string State { get; set; } = "Disponible";
    }

