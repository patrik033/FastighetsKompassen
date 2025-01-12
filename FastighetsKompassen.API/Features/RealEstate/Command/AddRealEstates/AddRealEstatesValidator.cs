using FluentValidation;

namespace FastighetsKompassen.API.Features.RealEstate.Command.AddRealEstates
{
    public class AddRealEstatesValidator : AbstractValidator<AddRealEstatesCommand>
    {
        public AddRealEstatesValidator()
        {
            RuleFor(x => x.RealEstateData)
            .NotEmpty()
            .WithMessage("Datan får inte vara tom");
        }
    }
}
