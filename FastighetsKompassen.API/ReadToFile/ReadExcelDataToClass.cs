using System.Globalization;
using FastighetsKompassen.Shared.Models.SkolData;
using OfficeOpenXml;

namespace FastighetsKompassen.API.ReadToFile
{
    public class ReadExcelDataToClass
    {
        public List<T> ReadSheetData<T>(Stream excelStream, int yearRange, string educationLevel) where T : class, new()
        {
            var results = new List<T>();
            using var package = new ExcelPackage(excelStream);
            var subjects = new[] { "Engelska", "Svenska", "Matematik" };

            foreach (var subject in subjects)
            {
                var worksheet = package.Workbook.Worksheets[subject];
                if (worksheet == null)
                {
                    Console.WriteLine($"Ingen data hittades för ämnet {subject}.");
                    continue;
                }

                int headerRow = worksheet.Cells["A1:A20"]
                    .FirstOrDefault(cell => cell.Text.Contains("Skola", StringComparison.OrdinalIgnoreCase))
                    ?.Start.Row ?? 10;

                int startRow = headerRow + 1;

                for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                {
                    try
                    {
                        var result = new T();
                        if (result is SchoolResultGradeSix gradeSixResult)
                        {
                            FillGradeSixData(worksheet, gradeSixResult, row, yearRange, educationLevel, subject);
                            results.Add(result as T);
                        }
                        else if (result is SchoolResultGradeNine gradeNineResult)
                        {
                            FillGradeNineData(worksheet, gradeNineResult, row, yearRange, educationLevel, subject);
                            results.Add(result as T);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Kunde inte läsa rad {row}: {ex.Message}");
                        continue;
                    }
                }
            }

            return results;
        }

        private void FillGradeSixData(ExcelWorksheet worksheet, SchoolResultGradeSix result, int row, int year, string educationLevel, string subject)
        {
            result.EndYear = year;
            result.EducationLevel = educationLevel;
            result.Subject = subject;

            result.SchoolName = worksheet.Cells[row, 1].Text;
            result.SchoolUnitCode = int.Parse(worksheet.Cells[row, 2].Text);
            result.SchoolMunicipality = worksheet.Cells[row, 3].Text;
            result.MunicipalityCode = int.Parse(worksheet.Cells[row, 4].Text);
            result.HeadOrganizationType = worksheet.Cells[row, 5].Text;
            result.HeadOrganizationName = worksheet.Cells[row, 6].Text;
            result.HeadOrganizationNumber = ParseNullableDouble(worksheet.Cells[row, 7].Text);
            result.TestCode = null;
            result.SubTest = worksheet.Cells[row, 8].Text;

            result.TotalParticipation = ParseNullableDouble(worksheet.Cells[row, 9].Text); // Som deltagit - Totalt
            result.FemaleParticipation = ParseNullableDouble(worksheet.Cells[row, 10].Text); // Som deltagit - Flickor
            result.MaleParticipation = ParseNullableDouble(worksheet.Cells[row, 11].Text); // Som deltagit - Pojkar
            result.TotalGradeAF = ParseNullableDouble(worksheet.Cells[row, 12].Text); // Provbetyg A-F Totalt
            result.FemaleGradeAF = ParseNullableDouble(worksheet.Cells[row, 13].Text); // Provbetyg A-F Flickor
            result.MaleGradeAF = ParseNullableDouble(worksheet.Cells[row, 14].Text); // Provbetyg A-F Pojkar
            result.TotalGradeAE = ParseNullableDouble(worksheet.Cells[row, 15].Text); // Provbetyg A-E Totalt
            result.FemaleGradeAE = ParseNullableDouble(worksheet.Cells[row, 16].Text); // Provbetyg A-E Flickor
            result.MaleGradeAE = ParseNullableDouble(worksheet.Cells[row, 17].Text); // Provbetyg A-E Pojkar
            result.GradePoints = ParseNullableDouble(worksheet.Cells[row, 18].Text); // Provbetygspoäng Totalt
            result.FemaleGradePoints = ParseNullableDouble(worksheet.Cells[row, 19].Text); // Provbetygspoäng Flickor
            result.MaleGradePoints = ParseNullableDouble(worksheet.Cells[row, 20].Text); // Provbetygspoäng Pojkar
        }

        private void FillGradeNineData(ExcelWorksheet worksheet, SchoolResultGradeNine result, int row, int year, string educationLevel, string subject)
        {
            result.EndYear = year;
            result.EducationLevel = educationLevel;
            result.Subject = subject;

            result.SchoolName = worksheet.Cells[row, 1].Text;
            result.SchoolUnitCode = int.Parse(worksheet.Cells[row, 2].Text);
            result.SchoolMunicipality = worksheet.Cells[row, 3].Text;
            result.MunicipalityCode = ParseNullableDouble(worksheet.Cells[row, 4].Text);
            result.HeadOrganizationType = worksheet.Cells[row, 5].Text;
            result.HeadOrganizationName = worksheet.Cells[row, 6].Text;
            result.HeadOrganizationNumber = ParseNullableDouble(worksheet.Cells[row, 7].Text);
          //  result.TestCode = worksheet.Cells[row, 8].Text;
            //result.SubTest = worksheet.Cells[row, 9].Text;

            result.TotalGradeAF = ParseNullableDouble(worksheet.Cells[row, 10].Text); // Provbetyg A-F Totalt
            result.FemaleGradeAF = ParseNullableDouble(worksheet.Cells[row, 11].Text); // Provbetyg A-F Flickor
            result.MaleGradeAF = ParseNullableDouble(worksheet.Cells[row, 12].Text); // Provbetyg A-F Pojkar
            result.TotalGradeAE = ParseNullableDouble(worksheet.Cells[row, 13].Text); // Provbetyg A-E Totalt
            result.FemaleGradeAE = ParseNullableDouble(worksheet.Cells[row, 14].Text); // Provbetyg A-E Flickor
            result.MaleGradeAE = ParseNullableDouble(worksheet.Cells[row, 15].Text); // Provbetyg A-E Pojkar
            result.GradePoints = ParseNullableDouble(worksheet.Cells[row, 16].Text); // Provbetygspoäng Totalt
            result.FemaleGradePoints = ParseNullableDouble(worksheet.Cells[row, 17].Text); // Provbetygspoäng Flickor
            result.MaleGradePoints = ParseNullableDouble(worksheet.Cells[row, 18].Text);
        }
        // Provbetygspoäng Pojkar


        public double? ParseNullableDouble(string input)
        {
            // Ignorera "~" och hantera tomma värden
            if (string.IsNullOrWhiteSpace(input) || input.Contains("~"))
            {
                input = input.Replace("~", ""); // Ta bort "~" men behåll värdet
            }

            // Byt ut komma mot punkt för att hantera olika decimaltecken
            input = input.Replace(",", ".");

            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return null; // Returnerar null om parsen misslyckas
        }
    }
}
