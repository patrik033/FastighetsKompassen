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
        }
    }
}
