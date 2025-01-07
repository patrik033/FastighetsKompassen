using FluentValidation;

namespace FastighetsKompassen.API.Features.School.Command
{
    public class AddSchoolResultValidator<T> : AbstractValidator<AddSchoolResultCommand<T>> where T : class
    {
        public AddSchoolResultValidator()
        {
            RuleFor(x => x.ExcelFile)
                .NotNull()
                .WithMessage("Ingen fil bifogades.")
                .Must(f => f.Length > 0)
                .WithMessage("Filen får inte vara tom.")
                .Must(f => f.FileName.EndsWith(".xlsx") || f.FileName.EndsWith(".xls"))
                .WithMessage("Filen måste vara en Excel-fil.");

            RuleFor(x => x.YearRange)
                .GreaterThan(0)
                .WithMessage("Årspannet måste vara ett positivt tal.");
        }
    }
}
