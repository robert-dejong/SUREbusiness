using Microsoft.EntityFrameworkCore;
using SUREbusiness.FleetManagement.DAL.Entities;

namespace SUREbusiness.FleetManagement.DAL
{
    public class VehiclesDbContext : DbContext
    {
        public VehiclesDbContext(DbContextOptions<VehiclesDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleEntity> Vehicles { get; set; }
    }
}