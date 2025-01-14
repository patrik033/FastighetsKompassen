using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{
    public record GetComparisonResultQuery(string Municipality1,string Municipality2, List<ComparisonParameter> Parameters) : IRequest<Result<List<ComparisonResultDTO>>>;
}
