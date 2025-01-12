using FastighetsKompassen.Shared.Models.ErrorHandling;
using FastighetsKompassen.Shared.Models.RealEstate;
using MediatR;

namespace FastighetsKompassen.API.Features.RealEstate.Command.AddRealEstates
{
    public record AddRealEstatesCommand(List<RealEstateData> RealEstateData) : IRequest<Result>;
}
