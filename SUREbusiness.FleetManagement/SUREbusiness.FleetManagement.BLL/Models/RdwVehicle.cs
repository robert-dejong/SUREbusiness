using Newtonsoft.Json;

namespace SUREbusiness.FleetManagement.BLL.Models
{
    public class RdwVehicle
    {
        [JsonProperty("kenteken")]
        public string LicensePlate { get; set; }
    }
}
