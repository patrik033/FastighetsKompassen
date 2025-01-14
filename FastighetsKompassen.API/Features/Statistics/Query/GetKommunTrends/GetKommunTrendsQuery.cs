using FastighetsKompassen.Shared.Models.DTO.Statistics;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends
{

    public record GetKommunTrendsQuery(string KommunId, int[] Year) : IRequest<Result<List<KommunTrendDto>>>;
}
