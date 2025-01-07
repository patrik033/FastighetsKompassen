using FluentValidation;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends
{
    public class GetKommunTrendsValidator : AbstractValidator<GetKommunTrendsQuery>
    {
        public GetKommunTrendsValidator()
        {
            RuleFor(query => query.KommunId)
                .NotEmpty()
                .WithMessage("There must be a municipldasd");

            RuleFor(query => query.Year)
                .NotEmpty()
                .WithMessage("Years cannot be null")
                .Must(x => x.Length > 0 && x.Length <= 5)
                .WithMessage("Number of elements must be between 1 and 5");

            RuleForEach(x => x.Year)
                .InclusiveBetween(2000, DateTime.Now.Year)
                .WithMessage("Each year must be between 2000 and the current year");


               

        }
    }
}
