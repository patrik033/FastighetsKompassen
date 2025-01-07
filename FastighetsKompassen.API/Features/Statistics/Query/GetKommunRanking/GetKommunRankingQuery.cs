using FastighetsKompassen.API.Features.Statistics.GetKommunRanking;
using MediatR;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking
{
    public record GetKommunRankingQuery(int Year, int Page,int PageSize) : IRequest<PaginatedResult<KommunRankingDto>>;
}
