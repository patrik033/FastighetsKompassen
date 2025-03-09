using FluentValidation;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById
{
    public class GetRealEstateByIdValidator : AbstractValidator<GetRealEstateByIdQuery>
    {
        public GetRealEstateByIdValidator()
        {
            RuleFor(x => x.RealEstateId)
            .NotEmpty()
            .WithMessage("RealEstateId får inte vara tom");

            RuleFor(x => x.RealEstateId)
           .NotEmpty().WithMessage("RealEstateId får inte vara tom")
           .GreaterThan(0).WithMessage("RealEstateId måste vara större än 0")
           .LessThanOrEqualTo(int.MaxValue).WithMessage("RealEstateId är för stort");
        }
    }
}
