using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Handlers;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Services;
using SUREbusiness.FleetManagement.DAL.Entities;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SUREbusiness.FleetManagement.BLL.UnitTests.Handlers
{
    public class CreateVehicleHandlerTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IValidator<Vehicle>> _validator;
        private readonly Mock<IVehicleLicensePlateExistsService> _vehicleLicensePlateExistsService;
        private readonly CreateVehicleHandler _createVehicleHandler;

        public CreateVehicleHandlerTests()
        {
            _vehicleRepository = new Mock<IVehicleRepository>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<Vehicle>>();
            _vehicleLicensePlateExistsService = new Mock<IVehicleLicensePlateExistsService>();

            _createVehicleHandler = new CreateVehicleHandler(_vehicleRepository.Object, 
                _mapper.Object, _validator.Object, _vehicleLicensePlateExistsService.Object);

            _vehicleLicensePlateExistsService.Setup(x => x.Exists(It.IsAny<string>())).ReturnsAsync(true);

            _validator.Setup(x => x.ValidateAsync(It.IsAny<Vehicle>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _mapper.Setup(x => x.Map<Vehicle>(It.IsAny<VehicleEntity>())).Returns(new Vehicle());
        }

        [Fact]
        public async Task GivenLicensePlateDoesNotExist_WhenHandleIsCalled_ThenBaseResultErrorIsReturned()
        {
            _vehicleLicensePlateExistsService.Setup(x => x.Exists(It.IsAny<string>())).ReturnsAsync(false);

            var command = new CreateVehicleCommand() {
                LicensePlate = "kenteken"
            };

            var result = await _createVehicleHandler.Handle(command, default);

            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task GivenVehicleValidationFails_WhenHandleIsCalled_ThenBaseResultErrorsAreReturned()
        {
            var validationErrors = new ValidationResult(
                new List<ValidationFailure>()
                {
                    new ValidationFailure("error", "error")
                });

            _validator.Setup(x => x.ValidateAsync(It.IsAny<Vehicle>(), 
                It.IsAny<CancellationToken>())).ReturnsAsync(validationErrors);

            var command = new CreateVehicleCommand()
            {
                LicensePlate = "34TDK2",
                LoanedTo = "Robert",
                Status = "beschikbaar",
                Remarks = "omschrijving",
                Color = "Rood",
                YearOfManufacture = 2000
            };

            var result = await _createVehicleHandler.Handle(command, default);

            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task GivenCreateVehicleCommand_WhenHandleIsCalled_ThenBaseResultVehicleIsReturned()
        {
            var result = await _createVehicleHandler.Handle(new CreateVehicleCommand(), default);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
        }
    }
}
