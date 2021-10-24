using SUREbusiness.FleetManagement.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Services
{
    public class VehicleLicensePlateExistsService : IVehicleLicensePlateExistsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VehicleLicensePlateExistsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Exists(string licensePlate)
        {
            var unformattedLicensePlate = licensePlate.ToUpper().Replace("-", "");
            var endpoint = $"/resource/m9d7-ebf2.json?kenteken={unformattedLicensePlate}";

            var httpClient = _httpClientFactory.CreateClient("rdw");
            var response = await httpClient.GetAsync(endpoint);
            var vehicle = await response.Content.ReadAsAsync<IEnumerable<RdwVehicle>>();

            return vehicle.Any(x => x.LicensePlate == unformattedLicensePlate);
        }
    }
}
