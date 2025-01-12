using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Kpi.Query.GetKPI
{
    public class GetKPIHandler : IRequestHandler<GetKPIQuery, Result<KpiDTO>>
    {

        private readonly AppDbContext _context;

        public GetKPIHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<KpiDTO>> Handle(GetKPIQuery request, CancellationToken cancellationToken)
        {
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
                     .Where(p => p.Kommun.Kommun == request.KommunId && p.Year == g.Key)
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

            var kpiReturnData = new KpiDTO
            {
                TotalSales = kpiData.TotalSales,
                MiddleAge = kpiData.MiddleAge,
                TotalCrimes = kpiData.TotalCrimes,
                AvgIncome = kpiData.AvgIncome
            };

            return Result<KpiDTO>.Success(kpiReturnData);
        }
    }
}
