using MediatR;
using SUREbusiness.FleetManagement.BLL.Models;

namespace SUREbusiness.FleetManagement.BLL.Queries
{
    public class GetVehicleByIdQuery : IRequest<Vehicle>
    {
        public int Id { get; private set; }

        public GetVehicleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
