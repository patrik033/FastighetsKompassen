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

        public async Task<PaginatedResult<KommunRankingDto>> GetKommunRankingAsync(int year, int page, int pageSize)
        {
            const decimal RealEstateMax = 5000M;
            const decimal LifeTimeMax = 1000M;
            const decimal AverageAgeMax = 1000M;
            const decimal CrimeMax = 50000M;
            const decimal SchoolMax = 5000M;

            const decimal RealEstateWeight = 0.1M;
            const decimal LifeTimeWeight = 0.1M;
            const decimal AverageAgeWeight = 0.1M;
            const decimal CrimeWeight = 0.4M;
            const decimal SchoolWeight = 0.3M;

            // Ladda data för aktuellt och föregående år
            var currentYearData = await LoadAggregatedData(year, RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);
            var previousYearData = await LoadAggregatedData(year - 1, RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);

            // Ladda alla kommuner
            var kommuner = await _context.Kommuner.AsNoTracking().ToListAsync();

            // Kombinera data och beräkna förändring
            var rankedKommuner = currentYearData
                .Select(data => new KommunRankingDto
                {
                    Kommun = data.Value.Kommun,
                    KommunNamn = kommuner.First(k => k.Id == data.Key).Kommunnamn,
                    TotalScore = Math.Round(data.Value.TotalScore ?? 0, 2), // Hanterar null-värden
                    ScoreChange = previousYearData.TryGetValue(data.Key, out var prevData)
                        ? Math.Round((data.Value.TotalScore ?? 0) - (prevData.TotalScore ?? 0), 2) // Hanterar null-värden
                        : (decimal?)null
                })
                .OrderByDescending(k => k.TotalScore)
                .ToList();


            // Paginerad data
            var totalCount = rankedKommuner.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = rankedKommuner.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<KommunRankingDto>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page
            };
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

        private async Task<Dictionary<int, AggregatedData>> LoadAggregatedData(
            int year,
            decimal realEstateMax,
            decimal lifeTimeMax,
            decimal averageAgeMax,
            decimal crimeMax,
            decimal schoolMax)
            {
                var realEstateData = await LoadRealEstateData(year);
                var middleAgeData = await LoadMiddleAgeData(year);
                var lifeTimeData = await LoadLifeTimeData(year);
                var crimeData = await LoadCrimeData(year);
                var schoolData = await LoadSchoolData(year);

                var kommuner = await _context.Kommuner.AsNoTracking().ToListAsync();

                return kommuner.ToDictionary(k => k.Id, k =>
                {
                    var realEstateScore = realEstateData.TryGetValue(k.Id, out var realEstate) ? (realEstate ?? 0) / realEstateMax : 0;
                    var ageScore = middleAgeData.TryGetValue(k.Id, out var age) ? (age ?? 0) / averageAgeMax : 0;
                    var lifeTimeScore = lifeTimeData.TryGetValue(k.Id, out var lifeTime) ? (lifeTime ?? 0) / lifeTimeMax : 0;

                    var crimeScore = crimeData.TryGetValue(k.Id, out var crime)
                        ? 1 - ((crime.SeriousCrimes * 0.6M + crime.MinorCrimes * 0.4M) / crimeMax)
                        : 0;

                    var schoolScore = schoolData.TryGetValue(k.Id, out var school)
                        ? (school.GradeNineScore + school.GradeSixScore) / 2 / schoolMax
                        : 0;

                    return new AggregatedData
                    {
                        TotalScore = realEstateScore * 0.1M +
                                     ageScore * 0.2M +
                                     lifeTimeScore * 0.3M +
                                     crimeScore * 0.3M +
                                     schoolScore * 0.1M,
                        Kommun = k.Kommun
                    };
                });
            }

        private async Task<Dictionary<int, decimal?>> LoadRealEstateData(int year)
        {
            return await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Year == year)
                .GroupBy(r => r.KommunId)
                .Select(g => new { KommunId = g.Key, AvgSales = g.Average(r => (decimal?)r.TotalSalesAmount / r.SalesCount) })
                .ToDictionaryAsync(r => r.KommunId, r => r.AvgSales);
        }


        private async Task<Dictionary<int, decimal?>> LoadMiddleAgeData(int year)
        {
            return await _context.AverageMiddleAge
                .AsNoTracking()
                .Where(a => a.Year == year)
                .GroupBy(a => a.KommunDataId)
                .Select(g => new { KommunId = g.Key, AvgAge = g.Average(a => (decimal?)a.Total) })
                .ToDictionaryAsync(a => a.KommunId, a => a.AvgAge);
        }

        private async Task<Dictionary<int, decimal?>> LoadLifeTimeData(int year)
        {
            return await _context.AverageLifeTime
                .AsNoTracking()
                .Where(l => l.YearSpan == "2019-2023")
                .GroupBy(l => l.KommunDataId)
                .Select(g => new { KommunId = g.Key, AvgLifeTime = g.Average(l => (decimal?)((l.MaleValue + l.FemaleValue) / 2)) })
                .ToDictionaryAsync(l => l.KommunId, l => l.AvgLifeTime);
        }


        private async Task<Dictionary<int, CrimeData>> LoadCrimeData(int year)
        {
            return await _context.PoliceEventSummary
                .AsNoTracking()
                .Where(pe => pe.Year == year)
                .GroupBy(pe => pe.KommunId)
                .Select(g => new
                {
                    KommunId = g.Key,
                    SeriousCrimes = g.Where(pe => pe.EventType == "Rån" || pe.EventType == "Mord/dråp").Sum(pe => (decimal?)pe.EventCount) ?? 0,
                    MinorCrimes = g.Where(pe => pe.EventType == "Småbrott").Sum(pe => (decimal?)pe.EventCount) ?? 0
                })
                .ToDictionaryAsync(c => c.KommunId, c => new CrimeData { SeriousCrimes = c.SeriousCrimes, MinorCrimes = c.MinorCrimes });
        }


        private async Task<Dictionary<int, SchoolData>> LoadSchoolData(int year)
        {
            var gradeNine = await _context.SchoolResultsGradeNine
                .AsNoTracking()
                .Where(s => s.StartYear == year)
                .GroupBy(s => s.KommunId)
                .Select(g => new { KommunId = g.Key, AvgScore = g.Average(s => (decimal?)s.GradePoints) })
                .ToDictionaryAsync(s => s.KommunId, s => s.AvgScore);

            var gradeSix = await _context.SchoolResultsGradeSix
                .AsNoTracking()
                .Where(s => s.StartYear == year)
                .GroupBy(s => s.KommunId)
                .Select(g => new { KommunId = g.Key, AvgScore = g.Average(s => (decimal?)s.GradePoints) })
                .ToDictionaryAsync(s => s.KommunId, s => s.AvgScore);

            return gradeNine.Keys.Union(gradeSix.Keys).ToDictionary(
                id => id,
                id => new SchoolData
                {
                    GradeNineScore = gradeNine.TryGetValue(id, out var nineScore) ? nineScore : 0,
                    GradeSixScore = gradeSix.TryGetValue(id, out var sixScore) ? sixScore : 0
                });
        }
    }
}
