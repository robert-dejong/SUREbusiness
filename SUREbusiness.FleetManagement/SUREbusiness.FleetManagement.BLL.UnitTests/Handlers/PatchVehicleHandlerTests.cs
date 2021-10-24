using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Handlers;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.DAL.Entities;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SUREbusiness.FleetManagement.BLL.UnitTests.Handlers
{
    public class PatchVehicleHandlerTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IValidator<Vehicle>> _validator;

        private readonly PatchVehicleHandler _patchVehicleHandler;
        private readonly PatchVehicleCommand _patchVehicleCommand;

        public PatchVehicleHandlerTests()
        {
            _vehicleRepository = new Mock<IVehicleRepository>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<Vehicle>>();
            _patchVehicleHandler = new PatchVehicleHandler(_vehicleRepository.Object, _mapper.Object, _validator.Object);

            var jsonPatchDocument = new JsonPatchDocument<VehiclePatchModel>();
            jsonPatchDocument.Replace(x => x.Status, "beschikbaar");

            _patchVehicleCommand = new PatchVehicleCommand(1, jsonPatchDocument);

            _mapper.Setup(x => x.Map<Vehicle>(It.IsAny<VehicleEntity>())).Returns(new Vehicle() { Status = "beschikbaar" });
            _mapper.Setup(x => x.Map<VehiclePatchModel>(It.IsAny<VehicleEntity>())).Returns(new VehiclePatchModel() { Status = "beschikbaar" });
        }

        [Fact]
        public async Task GivenValidatePatchFails_WhenHandleIsCalled_ThenBaseResultErrorIsReturned()
        {
            _mapper.Setup(x => x.Map<Vehicle>(It.IsAny<VehicleEntity>())).Returns(new Vehicle() { Status = "verkocht" });
            _mapper.Setup(x => x.Map<VehiclePatchModel>(It.IsAny<VehicleEntity>())).Returns(new VehiclePatchModel() { Status = "verkocht" });

            var result = await _patchVehicleHandler.Handle(_patchVehicleCommand, default);

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

            var result = await _patchVehicleHandler.Handle(_patchVehicleCommand, default);

            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task GivenPatchVehicleCommand_WhenHandleIsCalled_ThenBaseResultVehicleIsReturned()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<Vehicle>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var result = await _patchVehicleHandler.Handle(_patchVehicleCommand, default);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.Result);
        }
    }
}
