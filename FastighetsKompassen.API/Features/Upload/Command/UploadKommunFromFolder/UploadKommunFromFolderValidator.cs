using FluentValidation;

namespace FastighetsKompassen.API.Features.Upload.Command.UploadKommunFromFolder
{
    public class UploadKommunFromFolderValidator : AbstractValidator<UploadKommunFromFolderCommand>
    {
        public UploadKommunFromFolderValidator()
        {
            RuleFor(command => command.Letter)
           .NotEmpty()
           .WithMessage("Bokstaven får inte vara tom.")
           .Length(1)
           .WithMessage("Bokstaven måste vara exakt ett tecken lång.")
           .Must(letter => letter.Length == 1 && char.IsLetter(letter[0]))
           .WithMessage("Bokstaven måste vara en giltig bokstav mellan A-Ö.");
        }
    }
}
