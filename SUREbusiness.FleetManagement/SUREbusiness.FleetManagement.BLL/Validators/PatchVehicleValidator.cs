using FluentValidation;
using SUREbusiness.FleetManagement.BLL.Models;

namespace SUREbusiness.FleetManagement.BLL.Validators
{
    public class PatchVehicleValidator : AbstractValidator<VehiclePatchModel>
    {
        public PatchVehicleValidator(Vehicle vehicle)
        {
            RuleFor(vehiclePatchModel => vehiclePatchModel.Status)
                .Must(status => status == "verkocht")
                .When(x => vehicle.Status == "verkocht")
                .WithMessage("Status mag niet worden aangepast als het voertuig verkocht is");

            RuleFor(vehiclePatchModel => vehiclePatchModel.Status)
                .Must(x => vehicle.Status == "beschikbaar")
                .When(vehiclePatchModel => vehiclePatchModel.Status == "uitgeleend" && vehiclePatchModel.LoanedTo != vehicle.LoanedTo)
                .WithMessage("Voertuig mag alleen uitgeleend worden als de status beschikbaar is");
        }
    }
}
