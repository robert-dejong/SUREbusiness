using FluentValidation;
using FluentValidation.TestHelper;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Validators;
using System.Threading.Tasks;
using Xunit;

namespace SUREbusiness.FleetManagement.BLL.UnitTests.Validators
{
    public class VehicleValidatorTests
    {
        private IValidator<Vehicle> _vehicleValidator;

        public VehicleValidatorTests()
        {
            _vehicleValidator = new VehicleValidator();
        }

        [Fact]
        public async Task GivenLicensePlateIsEmpty_WhenValidated_ThenValidationFails()
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = string.Empty,
                Status = "beschikbaar"
            };

            var result = await _vehicleValidator.TestValidateAsync(vehicle);
            result.ShouldHaveValidationErrorFor(vehicle => vehicle.LicensePlate);
        }

        [Fact]
        public async Task GivenStatusDoesNotExist_WhenValidated_ThenValidationFails()
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = "34TDK2",
                Status = "niet bestaande status"
            };

            var result = await _vehicleValidator.TestValidateAsync(vehicle);
            result.ShouldHaveValidationErrorFor(vehicle => vehicle.Status);
        }

        [Fact]
        public async Task GivenStatusIsUitgeleendAndLoanedToIsEmpty_WhenValidated_ThenValidationFails()
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = "34TDK2",
                Status = "uitgeleend",
                LoanedTo = string.Empty
            };

            var result = await _vehicleValidator.TestValidateAsync(vehicle);
            result.ShouldHaveValidationErrorFor(vehicle => vehicle.LoanedTo);
        }

        [Fact]
        public async Task GivenStatusIsNotUitgeleendAndLoanedToIsNotEmpty_WhenValidated_ThenValidationFails()
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = "34TDK2",
                Status = "beschikbaar",
                LoanedTo = "Robert"
            };

            var result = await _vehicleValidator.TestValidateAsync(vehicle);
            result.ShouldHaveValidationErrorFor(vehicle => vehicle.LoanedTo);
        }

        [Fact]
        public async Task GivenVehicle_WhenValidated_ThenValidationSucceeds()
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = "34TDK2",
                Status = "uitgeleend",
                LoanedTo = "Robert",
                Color = "Rood",
                Remarks = "Test omschrijving",
                YearOfManufacture = 2000
            };

            var result = await _vehicleValidator.TestValidateAsync(vehicle);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
