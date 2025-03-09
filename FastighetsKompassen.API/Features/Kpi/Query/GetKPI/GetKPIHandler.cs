using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FastighetsKompassen.API.Features.Kpi.Query.GetKPI
{
    public class GetKPIHandler : IRequestHandler<GetKPIQuery, Result<KpiDTO>>
    {

        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public GetKPIHandler(AppDbContext context,IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Result<KpiDTO>> Handle(GetKPIQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"KPIData_{request.KommunId}";
            if(_cache.TryGetValue(cacheKey,out KpiDTO cachedKpiData))
            {
                return Result<KpiDTO>.Success(cachedKpiData);
            }

            var latestYear = await _context.PoliceEventSummary
                .Where(s => s.Kommun.Kommun == request.KommunId)
                .MaxAsync(s => s.Year);




            var kpiData = await _context.RealEstateYearlySummary
                 .AsNoTracking()
                 .Where(r => r.Kommun.Kommun == request.KommunId)
                 .GroupBy(r => r.Year)
                 .OrderByDescending(g => g.Key)
                 .Select(g => new
                 {
                     TotalSales = g.Sum(r => r.SalesCount),

                     TotalCrimes = _context.PoliceEventSummary
                     .AsNoTracking()
                     .Where(p => p.Kommun.Kommun == request.KommunId && p.Year == latestYear)
                     .Sum(p => p.EventCount),

                     MiddleAge = _context.AverageMiddleAge
                     .AsNoTracking()
                     .Where(m => m.Kommun.Kommun == request.KommunId && m.Year == g.Key)
                     .Select(m => m.Total)
                     .FirstOrDefault(),

                     AvgIncome = _context.Income
                        .AsNoTracking()
                        //240 == totalt
                        .Where(i => i.Kommun.Kommun == request.KommunId && i.IncomeComponent == "240")
                        .GroupBy(i => i.Year)
                        .OrderByDescending(g => g.Key)
                        .Select(g => g.Average(e => (decimal)e.MiddleValue))
                        .FirstOrDefault()

                 })
             .FirstOrDefaultAsync(cancellationToken);

            if (kpiData == null)
            {
                return Result<KpiDTO>.Failure("Ingen data hittades för angiven kommun.");
            }

            var kpiReturnData = new KpiDTO
            {
                TotalSales = kpiData.TotalSales,
                MiddleAge = kpiData.MiddleAge,
                TotalCrimes = kpiData.TotalCrimes,
                AvgIncome = kpiData.AvgIncome
            };

            _cache.Set(cacheKey, kpiReturnData, TimeSpan.FromMinutes(5));

            return Result<KpiDTO>.Success(kpiReturnData);
        }
    }
}
