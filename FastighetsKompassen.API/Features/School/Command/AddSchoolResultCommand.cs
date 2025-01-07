using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.School.Command
{
    public record AddSchoolResultCommand<T>(IFormFile ExcelFile, int YearRange) : IRequest<Result> where T : class;

}
