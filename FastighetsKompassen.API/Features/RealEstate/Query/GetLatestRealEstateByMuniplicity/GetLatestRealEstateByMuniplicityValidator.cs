using FluentValidation;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity
{
    public class GetLatestRealEstateByMuniplicityValidator : AbstractValidator<GetLatestRealEstateByMuniplicityQuery>
    {
        public GetLatestRealEstateByMuniplicityValidator()
        {
            RuleFor(x => x.KommunId)
           .NotEmpty()
           .WithMessage("KommunId får inte vara tom.");
          //.Length(8, 100).WithMessage("KommunId måste vara mellan 2 och 100 tecken.");
        }
    }
}
