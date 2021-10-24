using AutoMapper;
using FluentValidation;
using MediatR;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Services;
using SUREbusiness.FleetManagement.DAL.Entities;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Handlers
{
    public class CreateVehicleHandler : IRequestHandler<CreateVehicleCommand, BaseResult<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Vehicle> _validator;
        private readonly IVehicleLicensePlateExistsService _vehicleLicensePlateExistsService;

        public CreateVehicleHandler(IVehicleRepository vehicleRepository, IMapper mapper, IValidator<Vehicle> validator, IVehicleLicensePlateExistsService vehicleLicensePlateExistsService)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _validator = validator;
            _vehicleLicensePlateExistsService = vehicleLicensePlateExistsService;
        }

        public async Task<BaseResult<Vehicle>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleExists = await _vehicleLicensePlateExistsService.Exists(request.LicensePlate);

            if (!vehicleExists)
            {
                return new BaseResult<Vehicle>(error: $"License plate {request.LicensePlate} does not exist at RDW open data");
            }

            var vehicle = _mapper.Map<Vehicle>(request);
            var validator = await _validator.ValidateAsync(vehicle);

            if (!validator.IsValid)
            {
                return new BaseResult<Vehicle>(validator.Errors);
            }

            var vehicleEntity = _mapper.Map<VehicleEntity>(vehicle);
            var createdVehicleEntity = await _vehicleRepository.Add(vehicleEntity);
            var createdVehicle = _mapper.Map<Vehicle>(createdVehicleEntity);

            return new BaseResult<Vehicle>(createdVehicle);
        }
    }
}
