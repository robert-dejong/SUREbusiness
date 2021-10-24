using FluentValidation.TestHelper;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Validators;
using System.Threading.Tasks;
using Xunit;

namespace SUREbusiness.FleetManagement.BLL.UnitTests.Validators
{
    public class PatchVehicleValidatorTests
    {

        [Fact]
        public async Task GivenVehicleStatusIsVerkocht_WhenStatusIsChanged_ThenValidationFails()
        {
            var currentVehicle = new Vehicle()
            {
                Status = "verkocht"
            };

            var vehiclePatchModel = new VehiclePatchModel()
            {
                Status = "beschikbaar"
            };

            var validator = new PatchVehicleValidator(currentVehicle);

            var result = await validator.TestValidateAsync(vehiclePatchModel);
            result.ShouldHaveValidationErrorFor(vehiclePatchModel => vehiclePatchModel.Status);
        }

        [Fact]
        public async Task GivenVehicleStatusIsNotBeschikbaar_WhenStatusIsChangedToUitgeleend_ThenValidationFails()
        {
            var currentVehicle = new Vehicle()
            {
                Status = "in reparatie"
            };

            var vehiclePatchModel = new VehiclePatchModel()
            {
                Status = "uitgeleend",
                LoanedTo = "Robert"
            };

            var validator = new PatchVehicleValidator(currentVehicle);

            var result = await validator.TestValidateAsync(vehiclePatchModel);
            result.ShouldHaveValidationErrorFor(vehiclePatchModel => vehiclePatchModel.Status);
        }

        [Fact]
        public async Task GivenVehicleStatusIsBeschikbaar_WhenStatusIsChangedToUitgeleend_ThenValidationSucceeds()
        {
            var currentVehicle = new Vehicle()
            {
                Status = "beschikbaar"
            };

            var vehiclePatchModel = new VehiclePatchModel()
            {
                Status = "uitgeleend",
                LoanedTo = "Robert"
            };

            var validator = new PatchVehicleValidator(currentVehicle);

            var result = await validator.TestValidateAsync(vehiclePatchModel);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
