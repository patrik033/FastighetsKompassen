using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Runtime.CompilerServices;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{
    public class GetComparisonResultHandler : IRequestHandler<GetComparisonResultQuery, Result<List<ComparisonResultDTO>>>
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public GetComparisonResultHandler(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Result<List<ComparisonResultDTO>>> Handle(GetComparisonResultQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"ComparisonResultData_{request.Municipality1}_{request.Municipality2}_{string.Join(",", request.Parameters)}";

            // Kontrollera om resultatet redan finns i cache
            if (_cache.TryGetValue(cacheKey, out List<ComparisonResultDTO> cachedData))
            {
                return Result<List<ComparisonResultDTO>>.Success(cachedData);
            }



            var results = new List<ComparisonResultDTO>();

            foreach (var parameter in request.Parameters)
            {
                var result = await HandleComparison(parameter, request.Municipality1, request.Municipality2);

                if (!result.IsSuccess)
                {
                    return Result<List<ComparisonResultDTO>>.Failure(result.Error);
                }

                results.AddRange(result.Data);
            }



            if (results.Count > 0)
            {
                // Lägg till resultat i cache med en tidsbegränsning (t.ex. 10 minuters livslängd)
                _cache.Set(cacheKey, results, TimeSpan.FromMinutes(10));

                return Result<List<ComparisonResultDTO>>.Success(results);
            }

            return Result<List<ComparisonResultDTO>>.Failure("Ingen data returnerades, pröva en annan parameter");
        }


        private async Task<Result<List<ComparisonResultDTO>>> HandleComparison(
            ComparisonParameter parameter,
            string municipality1,
            string municipality2)
        {
            return parameter switch
            {
                ComparisonParameter.SchoolResults => await CompareSchoolResults(municipality1, municipality2),
                ComparisonParameter.PropertySales => await ComparePropertySales(municipality1, municipality2),
                ComparisonParameter.TotalSales => await CompareSingleResult(() => CompareTotalSales(municipality1, municipality2)),
                ComparisonParameter.TotalCrimes => await CompareSingleResult(() => CompareTotalCrimes(municipality1, municipality2)),
                ComparisonParameter.AvgIncome => await CompareSingleResult(() => CompareAvgIncome(municipality1, municipality2)),
                ComparisonParameter.AvgLifeExpectancy => await CompareSingleResult(() => CompareAvgLifeExpectancy(municipality1, municipality2)),
                _ => Result<List<ComparisonResultDTO>>.Failure("Ogiltig parameter angiven.")
            };
        }

        private async Task<Result<List<ComparisonResultDTO>>> CompareSingleResult(
            Func<Task<Result<ComparisonResultDTO>>> comparison)
        {
            var result = await comparison();
            return result.IsSuccess
                ? Result<List<ComparisonResultDTO>>.Success(new List<ComparisonResultDTO> { result.Data })
                : Result<List<ComparisonResultDTO>>.Failure(result.Error);
        }


        private async Task<Result<List<ComparisonResultDTO>>> CompareSchoolResults(string kommunId1, string kommunId2)
        {
            var results = new List<ComparisonResultDTO>();

            // Hämta skolresultat för första kommunen
            var schoolResults1 = await GetLatestSchoolResults(kommunId1);
            if (!schoolResults1.IsSuccess)
            {
                return Result<List<ComparisonResultDTO>>.Failure($"{schoolResults1.Error}");
            }

            // Hämta skolresultat för andra kommunen
            var schoolResults2 = await GetLatestSchoolResults(kommunId2);
            if (!schoolResults2.IsSuccess)
            {
                return Result<List<ComparisonResultDTO>>.Failure($"{schoolResults2.Error}");
            }

            // Skapa jämförelser för varje ämne som finns i båda kommunerna
            foreach (var subject in schoolResults1.Data.Keys)
            {
                if (schoolResults2.Data.ContainsKey(subject))
                {
                    results.Add(CreateComparisonResult(
                        ComparisonParameter.SchoolResults,
                        kommunId1,
                        kommunId2,
                        schoolResults1.Data[subject],
                        schoolResults2.Data[subject],
                        fieldName: subject // Lägg till ämnesnamnet
                    ));
                }
            }

            return Result<List<ComparisonResultDTO>>.Success(results);
        }


        private async Task<Result<List<ComparisonResultDTO>>> ComparePropertySales(string kommunId1, string kommunId2)
        {
            var results = new List<ComparisonResultDTO>();


            var propertySales1 = await GetLatestPropertySales(kommunId1);
            if (!propertySales1.IsSuccess)
            {
                return Result<List<ComparisonResultDTO>>.Failure($"Kunde inte hämta property sales för kommun {propertySales1}. Fel: {propertySales1.Error}");
            }
            var propertySales2 = await GetLatestPropertySales(kommunId2);
            if (!propertySales2.IsSuccess)
            {
                return Result<List<ComparisonResultDTO>>.Failure($"Kunde inte hämta property sales för kommun {propertySales2}. Fel: {propertySales2.Error}");
            }

            foreach (var propertyType in propertySales1.Data.Keys)
            {
                if (propertySales2.Data.ContainsKey(propertyType))
                {
                    results.Add(CreateComparisonResult(
                        ComparisonParameter.PropertySales,
                        kommunId1,
                        kommunId2,
                        propertySales1.Data[propertyType],
                        propertySales2.Data[propertyType],
                        fieldName: propertyType // Lägg till fastighetstyp
                    ));
                }
            }

            return Result<List<ComparisonResultDTO>>.Success(results);
        }

        private async Task<Result<ComparisonResultDTO>> CompareTotalSales(string kommunId1, string kommunId2)
        {
            var sales1 = await GetLatestTotalSales(kommunId1);
            if (!sales1.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {sales1}. Fel: {sales1.Error}");
            }
            var sales2 = await GetLatestTotalSales(kommunId2);
            if (!sales2.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {sales2}. Fel: {sales2.Error}");
            }


            var result = CreateComparisonResult(
                ComparisonParameter.TotalSales,
                kommunId1,
                kommunId2,
                sales1.Data,
                sales2.Data,
                fieldName: "Totala försäljningar");

            return Result<ComparisonResultDTO>.Success(result);
        }

        private async Task<Result<ComparisonResultDTO>> CompareTotalCrimes(string kommunId1, string kommunId2)
        {
            var crimes1 = await GetLatestTotalCrimes(kommunId1);
            if (!crimes1.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {crimes1}. Fel: {crimes1.Error}");
            }
            var crimes2 = await GetLatestTotalCrimes(kommunId2);
            if (!crimes2.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {crimes2}. Fel: {crimes2.Error}");
            }

            var result = CreateComparisonResult(
                ComparisonParameter.TotalCrimes,
                kommunId1,
                kommunId2,
                crimes1.Data,
                crimes2.Data,
                fieldName: "Totala brott");

            return Result<ComparisonResultDTO>.Success(result);
        }

        private async Task<Result<ComparisonResultDTO>> CompareAvgIncome(string kommunId1, string kommunId2)
        {
            var income1 = await GetLatestAvgIncome(kommunId1);
            if (!income1.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {income1}. Fel: {income1.Error}");
            }
            var income2 = await GetLatestAvgIncome(kommunId2);
            if (!income2.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {income2}. Fel: {income2.Error}");
            }
            var result = CreateComparisonResult(
                ComparisonParameter.AvgIncome,
                kommunId1,
                kommunId2,
                income1.Data,
                income2.Data,
                fieldName: "Genomsnittlig inkomst");
            return Result<ComparisonResultDTO>.Success(result);
        }

        private async Task<Result<ComparisonResultDTO>> CompareAvgLifeExpectancy(string kommunId1, string kommunId2)
        {
            var life1 = await GetLatestLifeExpectancy(kommunId1);
            if (!life1.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {life1}. Fel: {life1.Error}");
            }
            var life2 = await GetLatestLifeExpectancy(kommunId2);
            if (!life2.IsSuccess)
            {
                return Result<ComparisonResultDTO>.Failure($"Kunde inte hämta property sales för kommun {life2}. Fel: {life2.Error}");
            }

            var result = CreateComparisonResult(
                ComparisonParameter.AvgLifeExpectancy,
                kommunId1,
                kommunId2,
                life1.Data,
                life2.Data,
                fieldName: "Förväntad livslängd");
            return Result<ComparisonResultDTO>.Success(result);
        }

        private async Task<Result<decimal>> GetLatestTotalSales(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
            {
                return await _context.RealEstateYearlySummary
                    .Where(r => r.Kommun.Kommun == kommunId)
                    .GroupBy(r => r.Year)
                    .OrderByDescending(g => g.Key)
                    .Select(g => g.Sum(r => (decimal)r.SalesCount))
                    .FirstOrDefaultAsync();
            }, "");
        }



        private async Task<Result<decimal>> GetLatestTotalCrimes(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
            {

                return await _context.PoliceEventSummary
                    .Where(p => p.Kommun.Kommun == kommunId)
                    .GroupBy(p => p.Year)
                    .OrderByDescending(g => g.Key)
                    .Select(g => g.Sum(p => (decimal)p.EventCount))
                    .FirstOrDefaultAsync();
            }, "Ettt fel uppstod vid hämtning utav TotalCrimes");
        }


        private async Task<Result<decimal>> GetLatestAvgIncome(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
            {

                return await _context.Income
                    .Where(i => i.Kommun.Kommun == kommunId && i.IncomeComponent == "240")
                    .GroupBy(i => i.Year)
                    .OrderByDescending(g => g.Key)
                    .Select(g => g.Average(i => i.MiddleValue))
                    .FirstOrDefaultAsync();
            }, "Ett fel uppstod vid hämtning av AvgIncome");
        }

        private async Task<Result<decimal>> GetLatestLifeExpectancy(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
            {

                return await _context.AverageLifeTime
                    .Where(l => l.Kommun.Kommun == kommunId)
                    .OrderByDescending(l => l.YearSpan)
                    .Select(l => (l.MaleValue + l.FemaleValue) / 2)
                    .FirstOrDefaultAsync();

            }, "Ett fel uppstod vid hämtning av LifeExpectancy");
        }

        private async Task<Result<Dictionary<string, decimal>>> GetLatestSchoolResults(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
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
            }, "Ett fel uppstod vid hämtning av skolresultat");
        }

        private async Task<Result<Dictionary<string, decimal>>> GetLatestPropertySales(string kommunId)
        {
            return await ExecuteSafelyAsync(async () =>
            {
                var year = await _context.RealEstateYearlySummary
                   .Where(r => r.Kommun.Kommun == kommunId)
                   .MaxAsync(r => r.Year);


                var topSales = await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId && r.Year == year && r.PropertyType != null)
                .GroupBy(r => r.PropertyType)
                .Select(g => new
                {
                    PropertyType = g.Key,
                    TotalSalesCount = g.Sum(r => (decimal?)r.SalesCount) ?? 0
                })
                .OrderByDescending(x => x.TotalSalesCount)
                .Take(5)
                .ToListAsync();

                return topSales.ToDictionary(x => x.PropertyType!, x => x.TotalSalesCount);


            }, "Ett fel uppstod vid hämtning av propertysales");

        }

        private async Task<Result<T>> ExecuteSafelyAsync<T>(Func<Task<T>> action, string customMessage, [CallerMemberName] string methodName = "")
        {
            try
            {
                var result = await action();

                if (result == null || (result is ICollection collection && collection.Count == 0))
                {
                    return Result<T>.Failure($"[{methodName}] Resultatet var tomt eller null. {customMessage}");
                }

                return Result<T>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($" {methodName}  Fel: {ex.Message}");
            }
        }


        private ComparisonResultDTO CreateComparisonResult(ComparisonParameter parameter,
            string firstKommun,
            string secondKommun,
            decimal kommunFirstValue,
            decimal kommunSecondValue,
            string fieldName)
        {
            decimal percentageDifference;
            decimal timesLarger;

            if (kommunFirstValue == 0 && kommunSecondValue == 0)
            {
                percentageDifference = 0;
                timesLarger = 1; // Ingen skillnad
            }
            else if (kommunFirstValue == 0)
            {
                percentageDifference = -100;
                timesLarger = 0; // Kommun 1 har 0, så den är 0 gånger större
            }
            else if (kommunSecondValue == 0)
            {
                percentageDifference = 100;
                timesLarger = decimal.MaxValue; // Kommun 2 har 0, så Kommun 1 är "oändligt" större
            }
            else
            {
                // Beräkna procentuell skillnad baserat på det mindre värdet
                decimal maxValue = Math.Max(kommunFirstValue, kommunSecondValue);
                decimal minValue = Math.Min(kommunFirstValue, kommunSecondValue);

                percentageDifference = ((maxValue - minValue) / minValue) * 100;

                // Beräkna hur många gånger större den större kommunen är
                timesLarger = maxValue / minValue;

                // Om kommunSecondValue är större än kommunFirstValue, ska skillnaden vara positiv
                if (kommunSecondValue > kommunFirstValue)
                {
                    percentageDifference = Math.Abs(percentageDifference);
                }
                else
                {
                    percentageDifference = -Math.Abs(percentageDifference);
                }
            }

            return new ComparisonResultDTO
            {
                Parameter = parameter.ToString(),
                Municipality1 = firstKommun,
                Municipality2 = secondKommun,
                Value1 = kommunFirstValue,
                Value2 = kommunSecondValue,
                Difference = kommunFirstValue - kommunSecondValue,
                PercentageDifference = percentageDifference,
                TimesLarger = timesLarger, // Nytt fält för hur många gånger större den större kommunen är
                FieldName = fieldName
            };
        }


    }
}
