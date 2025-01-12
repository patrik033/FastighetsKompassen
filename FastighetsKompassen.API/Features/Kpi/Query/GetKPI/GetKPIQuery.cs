using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Kpi.Query.GetKPI
{
    public record GetKPIQuery(string KommunId) : IRequest<Result<KpiDTO>>;

}
