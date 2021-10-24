using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Queries;
using SUREbusiness.FleetManagement.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.BLL.Handlers
{
    public class GetVehiclesHandler : IRequestHandler<GetVehiclesQuery, Pagination<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public GetVehiclesHandler(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<Vehicle>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicleEntities = _vehicleRepository.GetAll();

            if(!string.IsNullOrEmpty(request.LoanedToFilter))
            {
                vehicleEntities = vehicleEntities.Where(entity => entity.LoanedTo == request.LoanedToFilter);
            }

            if (!string.IsNullOrEmpty(request.StatusFilter))
            {
                vehicleEntities = vehicleEntities.Where(entity => entity.Status == request.StatusFilter);
            }

            int totalVehicles = vehicleEntities.Count();

            if (request.Page > 0 && request.PageSize > 0)
            {
                int vehiclesToSkip = (request.Page - 1) * request.PageSize;
                vehicleEntities = vehicleEntities.Skip(vehiclesToSkip).Take(request.PageSize);
            }

            var vehicleEntityList = await vehicleEntities.ToListAsync();
            var vehicles = _mapper.Map<IEnumerable<Vehicle>>(vehicleEntityList);
            var pagination = new Pagination<Vehicle>(request.Page, request.PageSize, totalVehicles, vehicles);

            return pagination;
        }
    }
}
