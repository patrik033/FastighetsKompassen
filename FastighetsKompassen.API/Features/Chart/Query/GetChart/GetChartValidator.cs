using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using FluentValidation;
using MediatR;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart
{
    public class GetChartValidator : AbstractValidator<GetChartQuery>
    {
        public GetChartValidator()
        {
            RuleFor(x => x.KommunId)
            .NotEmpty()
            .WithMessage("Får inte vara tom")
            .MinimumLength(4)
            .WithMessage("Ogiltigt antal tecken");
        }
    }
}
