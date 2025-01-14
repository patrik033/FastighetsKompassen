using MediatR;
using Microsoft.EntityFrameworkCore;
using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;

namespace FastighetsKompassen.API.Features.Statistics.GetKommunRanking;

public class GetKommunRankingHandler : IRequestHandler<GetKommunRankingQuery, Result<PaginatedResult<KommunRankingDto>>>
{
    private readonly AppDbContext _context;

    public GetKommunRankingHandler(AppDbContext context)
    {
        _context = context;
    }



    public async Task<Result<PaginatedResult<KommunRankingDto>>> Handle(GetKommunRankingQuery request, CancellationToken cancellationToken)
    {
        const decimal RealEstateMax = 5000M;
        const decimal LifeTimeMax = 1000M;
        const decimal AverageAgeMax = 1000M;
        const decimal CrimeMax = 50000M;
        const decimal SchoolMax = 5000M;

        // 1) Hämta nuvarande och föregående års rådata
        var currentYearData = await LoadAggregatedData(request.Year,
            RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);
        var previousYearData = await LoadAggregatedData(request.Year - 1,
            RealEstateMax, LifeTimeMax, AverageAgeMax, CrimeMax, SchoolMax);

        var kommuner = await _context.Kommuner.AsNoTracking().ToListAsync();

        // 2) Beräkna rankning för föregående år (utan paginering)
        var previousYearRanks = previousYearData
            .OrderByDescending(x => x.Value.TotalScore ?? 0)
            .Select((x, index) => new
            {
                KommunId = x.Key,
                Rank = index + 1 // 1-baserad rank
            })
            .ToDictionary(x => x.KommunId, x => x.Rank);

        // 3) Beräkna rankning för innevarande år (utan paginering)
        //    Här sorterar vi efter TotalScore desc och sätter Rank = index + 1
        var currentYearRankData = currentYearData
            .OrderByDescending(x => x.Value.TotalScore ?? 0)
            .Select((x, index) => new
            {
                KommunId = x.Key,
                AggregatedData = x.Value,
                Rank = index + 1 // 1-baserad rank
            })
            .ToList();

        // 4) Bygg ihop en lista av KommunRankingDto
        //    - Räkna ut ScoreChange (år över år) som förr
        //    - Räkna ut RankChange med skillnad i rank
        var rankedKommuner = currentYearRankData
            .Select(x =>
            {
                var currentScore = x.AggregatedData.TotalScore ?? 0m;
                var prevScore = previousYearData.TryGetValue(x.KommunId, out var prevData)
                    ? (prevData.TotalScore ?? 0m)
                    : 0m;

                var kommunObj = kommuner.First(k => k.Id == x.KommunId);

                // RankChange => (Föregående rank) - (Nuvarande rank)
                var prevRank = previousYearRanks.TryGetValue(x.KommunId, out var pr)
                    ? pr
                    : (int?)null;

                int? rankChange = null;
                if (prevRank.HasValue)
                {
                    rankChange = prevRank.Value - x.Rank;
                }

                return new KommunRankingDto
                {
                    Kommun = x.AggregatedData.Kommun,
                    KommunNamn = kommunObj.Kommunnamn,
                    TotalScore = Math.Round(currentScore, 2),
                    ScoreChange = Math.Round(currentScore - prevScore, 2),

                    // Nya fält
                    Rank = x.Rank,
                    RankChange = rankChange
                };
            })
            .ToList();

        // 5) Nu har vi en helsorterad lista med "globala" ranker (1..N).
        //    Slutligen kör vi paginering:
        var totalCount = rankedKommuner.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = rankedKommuner
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();


        var paginetedResult = new PaginatedResult<KommunRankingDto>
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = request.Page
        };


        if(items.Count > 0)
        {
            return Result<PaginatedResult<KommunRankingDto>>.Success(paginetedResult);
        }
        return Result<PaginatedResult<KommunRankingDto>>.Failure("Ingen data returnerades, pröva en annan paramater");
      
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
