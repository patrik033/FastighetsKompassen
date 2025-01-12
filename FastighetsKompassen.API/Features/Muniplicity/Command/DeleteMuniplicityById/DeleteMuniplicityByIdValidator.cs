using FluentValidation;

namespace FastighetsKompassen.API.Features.Muniplicity.Command.DeleteMuniplicityById
{
    public class DeleteMuniplicityByIdValidator : AbstractValidator<DeleteMuniplicityByIdQuery>
    {
        public DeleteMuniplicityByIdValidator()
        {
            RuleFor(x => x.KommunId)
            .NotEmpty()
            .WithMessage("KommunId får ionte vara tom");
        }
    }
}
