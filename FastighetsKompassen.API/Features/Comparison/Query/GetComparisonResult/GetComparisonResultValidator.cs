using FluentValidation;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{
    public class GetComparisonResultValidator : AbstractValidator<GetComparisonResultQuery>
    {
        public GetComparisonResultValidator() 
        {
            RuleFor(query => query.Municipality1)
                .NotEmpty()
                .WithMessage("Municipality1 cannot be empty");

            RuleFor(query => query.Municipality2) 
                .NotEmpty()
                .WithMessage("Municipality2 cannot be empty");

            RuleFor(query => query.Municipality1)
                .NotEqual(x => x.Municipality2)
                .WithMessage("The two Municipalitys cannot be the same");

            RuleFor(query => query.Municipality2)
              .NotEqual(x => x.Municipality1)
              .WithMessage("The two Municipalitys cannot be the same");

            RuleFor(query => query.Parameters)
                .NotEmpty()
                .WithMessage("Parameters cannot be empty")
                .Must(x => x.Count > 0)
                .WithMessage("The collection needs to contain one or more items");
        }
    }
}
