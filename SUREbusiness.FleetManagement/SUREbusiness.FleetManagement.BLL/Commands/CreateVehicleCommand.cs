using MediatR;
using SUREbusiness.FleetManagement.BLL.Models;

namespace SUREbusiness.FleetManagement.BLL.Commands
{
    public class CreateVehicleCommand : IRequest<BaseResult<Vehicle>>
    {
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int YearOfManufacture { get; set; }
        public string LoanedTo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
