using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Services
{
    public interface IVehicleLicensePlateExistsService
    {
        Task<bool> Exists(string licensePlate);
    }
}