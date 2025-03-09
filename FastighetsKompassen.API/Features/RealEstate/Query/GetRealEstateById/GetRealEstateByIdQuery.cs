using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById
{
    public record GetRealEstateByIdQuery(int RealEstateId) : IRequest<Result<GetRealEstateByIdDTO>>;


}
