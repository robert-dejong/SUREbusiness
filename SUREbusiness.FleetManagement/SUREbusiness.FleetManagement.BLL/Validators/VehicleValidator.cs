using FluentValidation;
using SUREbusiness.FleetManagement.BLL.Models;
using System.Linq;

namespace SUREbusiness.FleetManagement.BLL.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        private static readonly string[] _validStatuses = { "uitgeleend", "beschikbaar", "in reparatie", "verkocht", "in bestelling" };

        public VehicleValidator()
        {
            var validStatuses = string.Join(", ", _validStatuses);

            RuleFor(vehicle => vehicle.LicensePlate).NotEmpty()
                .WithMessage("Kenteken mag niet leeg zijn");

            RuleFor(vehicle => vehicle.Status).Must(status => _validStatuses.Contains(status))
                .WithMessage($"Status mag alleen een van de volgende zijn: {validStatuses}");

            RuleFor(vehicle => vehicle.LoanedTo).NotEmpty().When(x => x.Status == "uitgeleend")
                .WithMessage("'uitgeleend aan' mag niet leeg zijn indien status 'uitgeleend' is");

            RuleFor(vehicle => vehicle.LoanedTo).Empty().When(x => x.Status != "uitgeleend")
                .WithMessage("'uitgeleend aan' moet leeg zijn indien status niet 'uitgeleend' is");
        }
    }
}
