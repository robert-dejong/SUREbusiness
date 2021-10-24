using SUREbusiness.FleetManagement.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.DAL.Repositories
{
    public interface IVehicleRepository
    {
        IQueryable<VehicleEntity> GetAll();
        Task<VehicleEntity> Get(int id);
        Task<VehicleEntity> Add(VehicleEntity vehicleEntity);
        Task<VehicleEntity> Update(VehicleEntity vehicleEntity);
    }
}