using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace FastighetsKompassen.API.Services
{
    public class StatisticsService
    {
        private readonly AppDbContext _context;
        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KommunRankingDto>> GetKommunRankingAsync(int year)
        {
            const decimal RealEstateMax = 10000000M;
            const decimal LifeTimeMax = 90M;
            const decimal AverageAgeMax = 50M;
            const decimal CrimeMax = 10000M;
            const decimal SchoolMax = 5M; // Maxpoäng för skolresultat (2 + 3)

            const decimal RealEstateWeight = 0.1M;
            const decimal LifeTimeWeight = 0.3M;
            const decimal AverageAgeWeight = 0.2M;
            const decimal CrimeWeight = 0.3M;
            const decimal SchoolWeight = 0.1M;

            var convertedYear = year.ToString();

            // Ladda all nödvändig data batchvis
            var realEstateData = await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Year == year)
                .GroupBy(r => r.KommunId)
                .Select(g => new { KommunId = g.Key, AvgSales = g.Average(r => (decimal?)r.TotalSalesAmount / r.SalesCount) })
                .ToDictionaryAsync(r => r.KommunId);

            var middleAgeData = await _context.AverageMiddleAge
                .AsNoTracking()
                .Where(a => a.Year == year)
                .GroupBy(a => a.KommunDataId)
                .Select(g => new { KommunId = g.Key, AvgAge = g.Average(a => (decimal?)a.Total) })
                .ToDictionaryAsync(a => a.KommunId);

            var lifeTimeData = await _context.AverageLifeTime
                .AsNoTracking()
                .Where(l => l.YearSpan == "2019-2023")
                .GroupBy(l => l.KommunDataId)
                .Select(g => new { KommunId = g.Key, AvgLifeTime = g.Average(l => (decimal?)((l.MaleValue + l.FemaleValue) / 2)) })
                .ToDictionaryAsync(l => l.KommunId);

            var crimeData = await _context.PoliceEventSummary
                .AsNoTracking()
                .Where(pe => pe.Year == year)
                .GroupBy(pe => pe.KommunId)
                .Select(g => new
                {
                    KommunId = g.Key,
                    SeriousCrimes = g.Where(pe => pe.EventType == "Rån" || pe.EventType == "Mord/dråp" || pe.EventType == "Mord/dråp, försök").Sum(pe => (decimal?)pe.EventCount) ?? 0,
                    MinorCrimes = g.Where(pe => pe.EventType == "Småbrott").Sum(pe => (decimal?)pe.EventCount) ?? 0
                })
                .ToDictionaryAsync(c => c.KommunId);

            var schoolDataYearNine = await _context.SchoolResultsGradeNine
                .AsNoTracking()
                .Where(s => s.StartYear == year)
                .GroupBy(s => s.KommunId)
                .Select(g => new
                {
                    KommunId = g.Key,
                    AvgScore = g.Average(s => (decimal?)s.GradePoints) // Betyg 2 + 3
                })
                .ToDictionaryAsync(s => s.KommunId);

            var schoolDataYearSix = await _context.SchoolResultsGradeSix
               .AsNoTracking()
               .Where(s => s.StartYear == year)
               .GroupBy(s => s.KommunId)
               .Select(g => new
               {
                   KommunId = g.Key,
                   AvgScore = g.Average(s => (decimal?)s.GradePoints) // Betyg 2 + 3
               })
               .ToDictionaryAsync(s => s.KommunId);

            // Hämta och beräkna rankingen
            var kommuner = await _context.Kommuner.AsNoTracking().ToListAsync();
            var result = kommuner
                .Select(k =>
                {
                    var realEstateScore = realEstateData.TryGetValue(k.Id, out var realEstate) ? (realEstate.AvgSales ?? 0) / RealEstateMax : 0;
                    var ageScore = middleAgeData.TryGetValue(k.Id, out var ageData) ? (ageData.AvgAge ?? 0) / AverageAgeMax : 0;
                    var lifeTimeScore = lifeTimeData.TryGetValue(k.Id, out var lifeTime) ? (lifeTime.AvgLifeTime ?? 0) / LifeTimeMax : 0;

                    var crimeScore = crimeData.TryGetValue(k.Id, out var crime)
                        ? 1 - ((crime.SeriousCrimes * 0.6M + crime.MinorCrimes * 0.4M) / CrimeMax)
                        : 0;

                    var schoolScoreNine = schoolDataYearNine.TryGetValue(k.Id, out var school) ? (school.AvgScore ?? 0) / SchoolMax : 0;
                    var schoolSchoreSix = schoolDataYearSix.TryGetValue(k.Id, out var schoolsix) ? (schoolsix.AvgScore ?? 0) / SchoolMax : 0;
                    return new KommunRankingDto
                    {
                        Kommun = k.Kommun,
                        KommunNamn = k.Kommunnamn,
                        TotalScore = (realEstateScore * RealEstateWeight) +
                                     (ageScore * AverageAgeWeight) +
                                     (lifeTimeScore * LifeTimeWeight) +
                                     (crimeScore * CrimeWeight) +
                                     (schoolScoreNine * SchoolWeight) +
                                     (schoolSchoreSix * SchoolWeight)
                    };
                })
                .OrderByDescending(k => k.TotalScore)
                .ToList();

            return result;
        }

        public async Task<List<KommunTrendDto>> GetKommunTrendsAsync(string kommunId, int[] years)
        {
            var results = new List<KommunTrendDto>();

            foreach (var year in years)
            {
                // Hämta livslängd
                var lifeTime = await _context.AverageLifeTime
                    .Where(l => l.Kommun.Kommun == kommunId && l.YearSpan == "2019-2023")
                    .AverageAsync(l => (decimal?)((l.MaleValue + l.FemaleValue) / 2)) ?? 0;

                // Hämta medelålder
                var averageAge = await _context.AverageMiddleAge
                    .Where(a => a.Kommun.Kommun == kommunId && a.Year == year)
                    .AverageAsync(a => (decimal?)a.Total) ?? 0;

                // Hämta fastighetsförsäljningar
                var realEstate = await _context.RealEstateYearlySummary
                    .Where(r => r.Kommun.Kommun == kommunId && r.Year == year)
                    .GroupBy(r => r.KommunId)
                    .Select(g => new
                    {
                        TotalSales = g.Sum(r => (decimal?)r.TotalSalesAmount) ?? 0,
                        SalesCount = g.Sum(r => (decimal?)r.SalesCount) ?? 0,
                        AvgSalesValue = g.Average(r => (decimal?)r.TotalSalesAmount / r.SalesCount) ?? 0
                    })
                    .FirstOrDefaultAsync();

                // Hämta brottsdata
                var crimeData = await _context.PoliceEventSummary
                    .Where(pe => pe.Kommun.Kommun == kommunId && pe.Year == year)
                    .GroupBy(pe => pe.EventType)
                    .Select(g => new { EventType = g.Key, Count = g.Sum(pe => (decimal?)pe.EventCount) ?? 0 })
                    .ToListAsync();

                // Hämta skolresultat (år 9 och 6)
                var schoolYearNine = await _context.SchoolResultsGradeNine
                    .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == year)
                    .AverageAsync(s => (decimal?)s.GradePoints) ?? 0;

                var schoolYearSix = await _context.SchoolResultsGradeSix
                    .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == year)
                    .AverageAsync(s => (decimal?)s.GradePoints) ?? 0;

                // Bygg upp resultat för detta år
                results.Add(new KommunTrendDto
                {
                    Year = year,
                    LifeTime = lifeTime,
                    AverageAge = averageAge,
                    TotalSales = realEstate?.TotalSales ?? 0,
                    SalesCount = realEstate?.SalesCount ?? 0,
                    AvgSalesValue = realEstate?.AvgSalesValue ?? 0,
                    Crimes = crimeData.ToDictionary(c => c.EventType, c => c.Count),
                    SchoolResultsYearNine = schoolYearNine,
                    SchoolResultsYearSix = schoolYearSix
                });
            }

            return results.OrderByDescending(r => r.Year).ToList();
        }





    }
}
