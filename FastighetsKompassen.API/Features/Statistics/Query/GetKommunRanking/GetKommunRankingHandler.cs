using MediatR;
using Microsoft.EntityFrameworkCore;
using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking;
using FastighetsKompassen.Infrastructure.Data;

namespace FastighetsKompassen.API.Features.Statistics.GetKommunRanking;

public class GetKommunRankingHandler : IRequestHandler<GetKommunRankingQuery, PaginatedResult<KommunRankingDto>>
{
    private readonly AppDbContext _context;

    public GetKommunRankingHandler(AppDbContext context)
    {
        _context = context;
    }



    public async Task<PaginatedResult<KommunRankingDto>> Handle(GetKommunRankingQuery request, CancellationToken cancellationToken)
    {
        const decimal RealEstateMax = 5000M;
        const decimal LifeTimeMax = 1000M;
        const decimal AverageAgeMax = 1000M;
        const decimal CrimeMax = 50000M;
        const decimal SchoolMax = 5000M;

        var currentYearData = await LoadAggregatedData(request.Year, RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);
        var previousYearData = await LoadAggregatedData(request.Year - 1, RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);

        var kommuner = await _context.Kommuner.AsNoTracking().ToListAsync();

        var rankedKommuner = currentYearData
            .Select(data => new KommunRankingDto
            {
                Kommun = data.Value.Kommun,
                KommunNamn = kommuner.First(k => k.Id == data.Key).Kommunnamn,
                TotalScore = Math.Round(data.Value.TotalScore ?? 0, 2),
                ScoreChange = previousYearData.TryGetValue(data.Key, out var prevData)
                    ? Math.Round((data.Value.TotalScore ?? 0) - (prevData.TotalScore ?? 0), 2)
                    : (decimal?)null
            })
            .OrderByDescending(k => k.TotalScore)
            .ToList();


        //paginated data
        var totalCount = rankedKommuner.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var items = rankedKommuner.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

        return new PaginatedResult<KommunRankingDto>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = request.Page
        };
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

    // Repositories för datahämtning
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
            .Where(s => s.EndYear == year)
            .GroupBy(s => s.KommunId)
            .Select(g => new { KommunId = g.Key, AvgScore = g.Average(s => (decimal?)s.GradePoints) })
            .ToDictionaryAsync(s => s.KommunId, s => s.AvgScore);

        var gradeSix = await _context.SchoolResultsGradeSix
            .AsNoTracking()
            .Where(s => s.EndYear == year)
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
