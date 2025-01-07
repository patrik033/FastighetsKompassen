using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{
    public class GetComparisonResultHandler : IRequestHandler<GetComparisonResultQuery, List<ComparisonResultDTO>>
    {
        private readonly AppDbContext _context;

        public GetComparisonResultHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComparisonResultDTO>> Handle(GetComparisonResultQuery request, CancellationToken cancellationToken)
        {
            var results = new List<ComparisonResultDTO>();

            foreach (var parameter  in request.Parameters)
            {
                switch (parameter)
                {
                    case ComparisonParameter.SchoolResults:
                        results.AddRange(await CompareSchoolResults(request.Municipality1, request.Municipality2));
                        break;
                    case ComparisonParameter.PropertySales:
                        results.AddRange(await ComparePropertySales(request.Municipality1, request.Municipality2));
                        break;
                    case ComparisonParameter.TotalSales:
                        results.Add(await CompareTotalSales(request.Municipality1, request.Municipality2));
                        break;
                    case ComparisonParameter.TotalCrimes:
                        results.Add(await CompareTotalCrimes(request.Municipality1, request.Municipality2));
                        break;
                    case ComparisonParameter.AvgIncome:
                        results.Add(await CompareAvgIncome(request.Municipality1, request.Municipality2));
                        break;
                    case ComparisonParameter.AvgLifeExpectancy:
                        results.Add(await CompareAvgLifeExpectancy(request.Municipality1, request.Municipality2));
                        break;
                }
            }
            return results;
        }

        private async Task<List<ComparisonResultDTO>> CompareSchoolResults(string kommunId1, string kommunId2)
        {
            var results = new List<ComparisonResultDTO>();
            var schoolResults1 = await GetLatestSchoolResults(kommunId1);
            var schoolResults2 = await GetLatestSchoolResults(kommunId2);

            foreach (var subject in schoolResults1.Keys)
            {
                if (schoolResults2.ContainsKey(subject))
                {
                    results.Add(CreateComparisonResult(
                        ComparisonParameter.SchoolResults,
                        kommunId1,
                        kommunId2,
                        schoolResults1[subject],
                        schoolResults2[subject],
                        fieldName: subject // Lägg till ämnesnamnet
                    ));
                }
            }

            return results;
        }

        private async Task<List<ComparisonResultDTO>> ComparePropertySales(string kommunId1, string kommunId2)
        {
            var results = new List<ComparisonResultDTO>();
            var propertySales1 = await GetLatestPropertySales(kommunId1);
            var propertySales2 = await GetLatestPropertySales(kommunId2);

            foreach (var propertyType in propertySales1.Keys)
            {
                if (propertySales2.ContainsKey(propertyType))
                {
                    results.Add(CreateComparisonResult(
                        ComparisonParameter.PropertySales,
                        kommunId1,
                        kommunId2,
                        propertySales1[propertyType],
                        propertySales2[propertyType],
                        fieldName: propertyType // Lägg till fastighetstyp
                    ));
                }
            }

            return results;
        }

        private async Task<ComparisonResultDTO> CompareTotalSales(string kommunId1, string kommunId2)
        {
            var sales1 = await GetLatestTotalSales(kommunId1);
            var sales2 = await GetLatestTotalSales(kommunId2);

            return CreateComparisonResult(
                ComparisonParameter.TotalSales,
                kommunId1,
                kommunId2,
                sales1,
                sales2,
                fieldName: "Totala försäljningar");
        }

        private async Task<ComparisonResultDTO> CompareTotalCrimes(string kommunId1, string kommunId2)
        {
            var crimes1 = await GetLatestTotalCrimes(kommunId1);
            var crimes2 = await GetLatestTotalCrimes(kommunId2);

            return CreateComparisonResult(
                ComparisonParameter.TotalCrimes,
                kommunId1,
                kommunId2,
                crimes1,
                crimes2,
                fieldName: "Totala brott");
        }

        private async Task<ComparisonResultDTO> CompareAvgIncome(string kommunId1, string kommunId2)
        {
            var income1 = await GetLatestAvgIncome(kommunId1);
            var income2 = await GetLatestAvgIncome(kommunId2);

            return CreateComparisonResult(
                ComparisonParameter.AvgIncome,
                kommunId1,
                kommunId2,
                income1,
                income2,
                fieldName: "Genomsnittlig inkomst");
        }

        private async Task<ComparisonResultDTO> CompareAvgLifeExpectancy(string kommunId1, string kommunId2)
        {
            var life1 = await GetLatestLifeExpectancy(kommunId1);
            var life2 = await GetLatestLifeExpectancy(kommunId2);

            return CreateComparisonResult(
                ComparisonParameter.AvgLifeExpectancy,
                kommunId1,
                kommunId2,
                life1,
                life2,
                fieldName: "Förväntad livslängd");
        }

        private async Task<decimal> GetLatestTotalSales(string kommunId)
        {
            return await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId)
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Sum(r => r.SalesCount))
                .FirstOrDefaultAsync();
        }

        private async Task<decimal> GetLatestTotalCrimes(string kommunId)
        {
            return await _context.PoliceEventSummary
                .Where(p => p.Kommun.Kommun == kommunId)
                .GroupBy(p => p.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Sum(p => (decimal)p.EventCount))
                .FirstOrDefaultAsync();
        }

        private async Task<decimal> GetLatestAvgIncome(string kommunId)
        {
            return await _context.Income
                .Where(i => i.Kommun.Kommun == kommunId && i.IncomeComponent == "240")
                .GroupBy(i => i.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Average(i => i.MiddleValue))
                .FirstOrDefaultAsync();
        }

        private async Task<decimal> GetLatestLifeExpectancy(string kommunId)
        {
            return await _context.AverageLifeTime
                .Where(l => l.Kommun.Kommun == kommunId)
                .OrderByDescending(l => l.YearSpan)
                .Select(l => (l.MaleValue + l.FemaleValue) / 2)
                .FirstOrDefaultAsync();
        }

        private async Task<Dictionary<string, decimal>> GetLatestSchoolResults(string kommunId)
        {
            var latestYear = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId)
                .MaxAsync(s => s.EndYear);

            return await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId && s.EndYear == latestYear)
                .GroupBy(s => s.Subject)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => (decimal)(g.Average(s => (double?)s.GradePoints) ?? 0)
                );
        }

        private async Task<Dictionary<string, decimal>> GetLatestPropertySales(string kommunId)
        {
            var year = await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId)
                .MaxAsync(r => r.Year);

            return await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId && r.Year == year)
                .GroupBy(r => r.PropertyType)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => (decimal)g.Sum(r => r.SalesCount)
                );
        }

        private ComparisonResultDTO CreateComparisonResult(
            ComparisonParameter parameter,
            string kommun1,
            string kommun2,
            decimal value1,
            decimal value2,
            string fieldName)
        {
            return new ComparisonResultDTO
            {
                Parameter = parameter.ToString(), // Enum converted to its name
                Municipality1 = kommun1,
                Municipality2 = kommun2,
                Value1 = value1,
                Value2 = value2,
                Difference = value1 - value2,
                PercentageDifference = value2 != 0 ? ((value1 - value2) / value2) * 100 : 0,
                FieldName = fieldName // Optional field for additional details
            };
        }
    }
}
