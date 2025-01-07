using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity
{
    public record GetLatestRealEstateByMuniplicityQuery(string KommunId) : IRequest<Result<List<GetLatestRealEstatateByMuniplicityDTO>>>;
}
