using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.SkolData;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;


namespace FastighetsKompassen.API.Services
{
    public class SchoolService
    {
        private readonly AppDbContext _dbContext;
        private readonly ReadExcelDataToClass _excelReader;

        public SchoolService(AppDbContext dbContext, ReadExcelDataToClass excelReader)
        {
            _dbContext = dbContext;
            _excelReader = excelReader;
        }





        //public async Task<bool> AddSchoolDataForYearSixAsync(Stream excelStream, int startYear)
        //{
        //    return await AddSchoolDataAsync<SchoolResultGradeSix>(excelStream, startYear, "Year6");
        //}

        //public async Task<bool> AddSchoolDataForYearNineAsync(Stream excelStream, int startYear)
        //{
        //    return await AddSchoolDataAsync<SchoolResultGradeNine>(excelStream, startYear, "Year9");
        //}

        //private async Task<bool> AddSchoolDataAsync<T>(Stream excelStream, int startYear, string educationLevel) where T : class
        //{
        //    using var package = new ExcelPackage(excelStream);
        //    var schoolResults = new List<T>();

        //    var worksheetEnglish = package.Workbook.Worksheets["Engelska"];
        //    var worksheetSwedish = package.Workbook.Worksheets["Svenska"];
        //    var worksheetMatematik = package.Workbook.Worksheets["Matematik"];

        //    // Läs och lägg till data från kalkylblad
        //    if (worksheetEnglish != null) _excelReader.ReadSheetData(worksheetEnglish, schoolResults, educationLevel, startYear);
        //    if (worksheetSwedish != null) _excelReader.ReadSheetData(worksheetSwedish, schoolResults, educationLevel, startYear);
        //    if (worksheetMatematik != null) _excelReader.ReadSheetData(worksheetMatematik, schoolResults, educationLevel, startYear);

        //    if (!schoolResults.Any())
        //    {
        //        return false; // Ingen data att lägga till
        //    }

        //    foreach (var result in schoolResults)
        //    {
        //        if (result is SchoolResultGradeSix gradeSixResult)
        //        {
        //            await AddToDatabaseIfNotExistsAsync(gradeSixResult.MunicipalityCode, gradeSixResult);
        //        }
        //        else if (result is SchoolResultGradeNine gradeNineResult)
        //        {
        //            await AddToDatabaseIfNotExistsAsync(gradeNineResult.MunicipalityCode, gradeNineResult);
        //        }
        //    }

        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task AddSchoolResultsAsync<T>(IEnumerable<T> schoolResults) where T : class
        {
            var kommunIds = _dbContext.Kommuner.ToDictionary(k => k.Kommun, k => k.Id);
            var resultsToSave = new List<T>();

            foreach (var result in schoolResults)
            {
                if (result is SchoolResultGradeSix gradeSixResult)
                {
                    if (kommunIds.TryGetValue(gradeSixResult.MunicipalityCode?.ToString(), out var kommunId))
                    {
                        gradeSixResult.KommunId = kommunId;
                        resultsToSave.Add(result);
                    }
                }
                else if (result is SchoolResultGradeNine gradeNineResult)
                {
                    if (kommunIds.TryGetValue(gradeNineResult.MunicipalityCode?.ToString(), out var kommunId))
                    {
                        gradeNineResult.KommunId = kommunId;
                        resultsToSave.Add(result);
                    }
                }
            }

            if (resultsToSave.Any())
            {
                _dbContext.AddRange(resultsToSave);
                await _dbContext.SaveChangesAsync();
            }
        }



        private async Task AddToDatabaseIfNotExistsAsync(double? municipalityCode, object schoolResult)
        {
            if (municipalityCode == null)
            {
                Console.WriteLine("Municipality code saknas.");
                return;
            }

            var kommun = await _dbContext.Kommuner
                .FirstOrDefaultAsync(k => k.Kommun == municipalityCode.ToString());

            if (kommun == null)
            {
                Console.WriteLine($"Ingen kommun hittades för kod {municipalityCode}");
                return;
            }

            if (schoolResult is SchoolResultGradeSix gradeSixResult)
            {
                gradeSixResult.KommunId = kommun.Id;
                _dbContext.SchoolResultsGradeSix.Add(gradeSixResult);
            }
            else if (schoolResult is SchoolResultGradeNine gradeNineResult)
            {
                gradeNineResult.KommunId = kommun.Id;
                _dbContext.SchoolResultsGradeNine.Add(gradeNineResult);
            }

            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Data har lagts till i databasen.");
        }

    }
}
