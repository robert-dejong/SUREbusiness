using Microsoft.EntityFrameworkCore;
using SUREbusiness.FleetManagement.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.DAL.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehiclesDbContext _vehiclesDbContext;

        public VehicleRepository(VehiclesDbContext vehiclesDbContext)
        {
            _vehiclesDbContext = vehiclesDbContext;
        }

        public IQueryable<VehicleEntity> GetAll()
        {
            return _vehiclesDbContext.Vehicles.AsNoTracking();
        }

        public async Task<VehicleEntity> Get(int id)
        {
            return await _vehiclesDbContext.Vehicles
                .AsNoTracking().FirstOrDefaultAsync(vehicle => vehicle.Id == id);
        }

        public async Task<VehicleEntity> Add(VehicleEntity vehicleEntity)
        {
            _ = await _vehiclesDbContext.Vehicles.AddAsync(vehicleEntity);
            _ = await _vehiclesDbContext.SaveChangesAsync();

            return vehicleEntity;
        }

        public async Task<VehicleEntity> Update(VehicleEntity vehicleEntity)
        {
            _ = _vehiclesDbContext.Vehicles.Update(vehicleEntity);
            _ = await _vehiclesDbContext.SaveChangesAsync();

            return vehicleEntity;
        }
    }
}
