using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SUREbusiness.FleetManagement.BLL.Models;

namespace SUREbusiness.FleetManagement.BLL.Commands
{
    public class PatchVehicleCommand : IRequest<BaseResult<Vehicle>>
    {
        public int Id { get; private set; }
        public JsonPatchDocument<VehiclePatchModel> VehiclePatchModelDocument { get; private set; }

        public PatchVehicleCommand(int id, JsonPatchDocument<VehiclePatchModel> vehiclePatchModelDocument)
        {
            Id = id;
            VehiclePatchModelDocument = vehiclePatchModelDocument;
        }
    }
}
