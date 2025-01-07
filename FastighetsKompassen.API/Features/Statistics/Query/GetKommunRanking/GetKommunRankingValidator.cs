using FluentValidation;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking
{
    public class GetKommunRankingValidator : AbstractValidator<GetKommunRankingQuery>
    {
        public GetKommunRankingValidator()
        {
            RuleFor(query => query.Year)
                .InclusiveBetween(2000, DateTime.Now.Year)
                .WithMessage("Year must be between 2000 and the current year.");
            RuleFor(query => query.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");
            RuleFor(query => query.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
