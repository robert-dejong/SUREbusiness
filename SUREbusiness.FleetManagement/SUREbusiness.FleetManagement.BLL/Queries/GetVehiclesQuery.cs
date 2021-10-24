using MediatR;
using SUREbusiness.FleetManagement.BLL.Models;

namespace SUREbusiness.FleetManagement.BLL.Queries
{
    public class GetVehiclesQuery : IRequest<Pagination<Vehicle>>
    {
        public string LoanedToFilter { get; private set; }
        public string StatusFilter { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetVehiclesQuery(string loanedToFilter, string statusFilter, int page, int pageSize)
        {
            LoanedToFilter = loanedToFilter;
            StatusFilter = statusFilter;
            Page = page;
            PageSize = pageSize;
        }
    }
}
