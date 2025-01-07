using FastighetsKompassen.Shared.Models.DTO.Statistics;
using MediatR;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends
{

    public record GetKommunTrendsQuery(string KommunId, int[] Year) : IRequest<List<KommunTrendDto>>;
}
