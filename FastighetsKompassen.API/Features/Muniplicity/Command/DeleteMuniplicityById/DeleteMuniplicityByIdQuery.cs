using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Muniplicity.Command.DeleteMuniplicityById
{
    public record DeleteMuniplicityByIdQuery(int KommunId): IRequest<Result>;
}
