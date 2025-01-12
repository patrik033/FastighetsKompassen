using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById
{
    public record GetRealEstateByIdQuery(int RealEstateId) : IRequest<Result<GetRealEstateByIdDTO>>;

}
