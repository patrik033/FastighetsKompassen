using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using FastighetsKompassen.Shared.Models.SkolData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.School.Command
{
    using FastighetsKompassen.Infrastructure.Data;
    using FastighetsKompassen.Shared.Models.SkolData;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;

    public class AddSchoolResultHandler<T> : IRequestHandler<AddSchoolResultCommand<T>, Result> where T : class
    {
        private readonly AppDbContext _context;
        private readonly ReadExcelDataToClass readExcelDataToClass;

        public AddSchoolResultHandler(AppDbContext context)
        {
            _context = context;
            readExcelDataToClass = new ReadExcelDataToClass();
        }

        public async Task<Result> Handle(AddSchoolResultCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                // Läs data från Excel-filen
                var schoolResults = ReadExcelData(request.ExcelFile, request.YearRange);

                if (!schoolResults.Any())
                {
                    return Result.Failure("Ingen data kunde läsas från filen.");
                }

                // Lägg till data i databasen
                await AddSchoolResultsAsync(schoolResults);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Ett fel inträffade vid bearbetning: {ex.Message}");
            }
        
        }

        private IEnumerable<T> ReadExcelData(IFormFile excelFile, int yearRange)
        {
            using var stream = excelFile.OpenReadStream();

            if (typeof(T) == typeof(SchoolResultGradeSix))
            {
                var result = new ReadExcelDataToClass().ReadSheetData<SchoolResultGradeSix>(stream, yearRange, "Årskurs 6");
                return result.Cast<T>();
            }
            else if (typeof(T) == typeof(SchoolResultGradeNine))
            {
                var result = new ReadExcelDataToClass().ReadSheetData<SchoolResultGradeNine>(stream, yearRange, "Årskurs 9");
                return result.Cast<T>();
            }
            else
            {
                throw new ArgumentException("Ogiltig datatyp. Endast årskurs 6 och 9 stöds.");
            }
        }

        private async Task AddSchoolResultsAsync(IEnumerable<T> schoolResults)
        {
            var kommunIds = _context.Kommuner.ToDictionary(k => k.Kommun, k => k.Id);
            var resultsToSave = new List<T>();

            foreach (var result in schoolResults)
            {
                switch (result)
                {
                    case SchoolResultGradeSix gradeSixResult:
                        if (kommunIds.TryGetValue(gradeSixResult.MunicipalityCode?.ToString(), out var kommunIdSix))
                        {
                            gradeSixResult.KommunId = kommunIdSix;
                            resultsToSave.Add(result);
                        }
                        break;

                    case SchoolResultGradeNine gradeNineResult:
                        if (kommunIds.TryGetValue(gradeNineResult.MunicipalityCode?.ToString(), out var kommunIdNine))
                        {
                            gradeNineResult.KommunId = kommunIdNine;
                            resultsToSave.Add(result);
                        }
                        break;
                }
            }

            if (resultsToSave.Any())
            {
                _context.AddRange(resultsToSave);
                await _context.SaveChangesAsync();
            }
        }
    }


}
