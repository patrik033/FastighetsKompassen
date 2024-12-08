using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FastighetsKompassen.API.Services
{
    public class KPIService
    {
        private readonly AppDbContext _context;

        public KPIService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<KpiDTO> GetKPIAsync(string kommunId)
        {

            var kpiData = await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Kommun.Kommun == kommunId)
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new
                {
                    TotalSales = g.Sum(r => r.SalesCount),

                    TotalCrimes = _context.PoliceEventSummary
                    .AsNoTracking()
                    .Where(p => p.Kommun.Kommun == kommunId && p.Year == g.Key)
                    .Sum(p => p.EventCount),

                    MiddleAge = _context.AverageMiddleAge
                    .AsNoTracking()
                    .Where(m => m.Kommun.Kommun == kommunId && m.Year == g.Key)
                    .Select(m => m.Total)
                    .FirstOrDefault(),

                    AvgIncome = _context.Income
                       .AsNoTracking()
                       //240 == totalt
                       .Where(i => i.Kommun.Kommun == kommunId && i.IncomeComponent == "240")
                       .GroupBy(i => i.Year)
                       .OrderByDescending(g => g.Key)
                       .Select(g => g.Average(e => (decimal)e.MiddleValue))
                       .FirstOrDefault()

                })
            .FirstOrDefaultAsync();

            return new KpiDTO
            {
                TotalSales = kpiData.TotalSales,
                MiddleAge = kpiData.MiddleAge,
                TotalCrimes = kpiData.TotalCrimes,
                AvgIncome = kpiData.AvgIncome
            };
        }
    }
}
