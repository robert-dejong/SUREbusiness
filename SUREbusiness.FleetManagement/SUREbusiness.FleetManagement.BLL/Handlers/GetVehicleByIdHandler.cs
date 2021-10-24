using AutoMapper;
using MediatR;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Queries;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Handlers
{
    public class GetVehicleByIdHandler : IRequestHandler<GetVehicleByIdQuery, Vehicle>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public GetVehicleByIdHandler(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<Vehicle> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicleEntity = await _vehicleRepository.Get(request.Id);
            var vehicle = _mapper.Map<Vehicle>(vehicleEntity);

            return vehicle;
        }
    }
}
