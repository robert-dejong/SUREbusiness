using AutoMapper;
using FluentValidation;
using MediatR;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Validators;
using SUREbusiness.FleetManagement.DAL.Entities;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Handlers
{
    public class PatchVehicleHandler : IRequestHandler<PatchVehicleCommand, BaseResult<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Vehicle> _validator;

        public PatchVehicleHandler(IVehicleRepository vehicleRepository, IMapper mapper, IValidator<Vehicle> validator)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<BaseResult<Vehicle>> Handle(PatchVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleEntity = await _vehicleRepository.Get(request.Id);
            var vehicle = _mapper.Map<Vehicle>(vehicleEntity);
            var vehiclePatchModel = _mapper.Map<VehiclePatchModel>(vehicleEntity);

            request.VehiclePatchModelDocument.ApplyTo(vehiclePatchModel);

            var validatePatch = new PatchVehicleValidator(vehicle).Validate(vehiclePatchModel);

            if (!validatePatch.IsValid)
            {
                return new BaseResult<Vehicle>(validatePatch.Errors);
            }

            vehicle.LoanedTo = vehiclePatchModel.LoanedTo;
            vehicle.Status = vehiclePatchModel.Status;

            var validator = await _validator.ValidateAsync(vehicle);

            if (!validator.IsValid)
            {
                return new BaseResult<Vehicle>(validator.Errors);
            }

            var vehicleToUpdate = _mapper.Map<VehicleEntity>(vehicle);
            _ = await _vehicleRepository.Update(vehicleToUpdate);

            return new BaseResult<Vehicle>(vehicle);
        }
    }
}
