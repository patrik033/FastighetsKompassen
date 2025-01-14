using FastighetsKompassen.API.Features.Chart.Query.GetChart.ChartDtos;
using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart
{
    public record GetChartQuery(string KommunId) : IRequest<Result<ChartDataDTO>>;
}
