using FluentValidation;

namespace FastighetsKompassen.API.Features.Kpi.Query.GetKPI
{
    public class GetKPIValidator : AbstractValidator<GetKPIQuery>
    {
        public GetKPIValidator()
        {
            RuleFor(x => x.KommunId)
            .NotEmpty()
            .WithMessage("KommunId får inte vara tom")
            .MinimumLength(4)
            .WithMessage("Ogiltigt antal tecken");
        }
    }
}
