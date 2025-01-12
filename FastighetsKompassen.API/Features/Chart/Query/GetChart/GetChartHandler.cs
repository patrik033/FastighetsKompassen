using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart
{
    public class GetChartHandler : IRequestHandler<GetChartQuery, Result<ChartDataDTO>>
    {

        private readonly AppDbContext _context;

        public GetChartHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ChartDataDTO>> Handle(GetChartQuery request, CancellationToken cancellationToken)
        {
            var latestYear = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == request.KommunId)
                .MaxAsync(s => s.EndYear);

            var result = new ChartDataDTO
            {
                PropertySales = await GetPropertySales(request.KommunId),

                CrimeDistribution = await GetCrimeDistribution(request.KommunId, latestYear)
                .Take(5)
                .ToListAsync(cancellationToken),

                AvgIncome = await GetIncomeData(request.KommunId)
                .Take(5)
                .ToListAsync(cancellationToken),

                AvgLifeExpectancy = await GetLifeExpectancy(request.KommunId)
                .FirstOrDefaultAsync(cancellationToken),

                SchoolResultYearNine = await GetSchoolResults(request.KommunId, latestYear)
                .ToListAsync(cancellationToken),

                TopSchools = await GetTopSchools(request.KommunId, latestYear)
                .ToListAsync(cancellationToken)
            };

            if (result is null)
                return Result<ChartDataDTO>.Failure("Ingen data returnerades, pröva en annan parameter");
            else
                return Result<ChartDataDTO>.Success(result);
        }


        private async Task<List<PropertySalesDTO>> GetPropertySales(string kommunId)
        {
            // Hämta det senaste året för vald kommun
            var latestYear = await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Kommun.Kommun == kommunId)
                .MaxAsync(r => r.Year);

            // Hämta data för senaste året och aggregera resultaten
            var propertySales = await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Kommun.Kommun == kommunId &&
                            r.Year == latestYear &&
                            (r.PropertyType == "Villa" || r.PropertyType == "Lägenhet" || r.PropertyType == "Radhus"))
                .GroupBy(r => r.PropertyType)
                .Select(ps => new PropertySalesDTO
                {
                    PropertyType = ps.Key,
                    SalesCount = ps.Sum(r => r.SalesCount),
                    TotalSalesAmount = ps.Sum(r => r.TotalSalesAmount)
                })
                .ToListAsync();

            return propertySales;
        }


        private IQueryable<CrimeDistributionDTO> GetCrimeDistribution(string kommunId, int year)
        {
            return _context.PoliceEventSummary
                .AsNoTracking()
                .Where(p => p.Kommun.Kommun == kommunId && p.Year == year)
                .GroupBy(p => p.EventType)
                .OrderByDescending(cd => cd.Sum(e => e.EventCount)) // Sortering direkt efter GroupBy
                .Select(cd => new CrimeDistributionDTO
                {
                    EventType = cd.Key,
                    EventCount = cd.Sum(e => e.EventCount)
                })
                .Take(5);
        }

        private IQueryable<IncomeDTO> GetIncomeData(string kommunId)
        {
            return _context.Income
                .AsNoTracking()
                .Where(i => i.Kommun.Kommun == kommunId && i.IncomeComponent == "240")
                .GroupBy(i => i.Year)
                .OrderByDescending(i => i.Key)
                .Select(ai => new IncomeDTO
                {
                    Year = ai.Key,
                    AvgIncome = ai.Average(i => i.MiddleValue)
                });
        }

        private IQueryable<LifeExpectancyDTO> GetLifeExpectancy(string kommunId)
        {
            return _context.AverageLifeTime
                .AsNoTracking()
                .Where(l => l.Kommun.Kommun == kommunId)
                .Select(le => new LifeExpectancyDTO
                {
                    Total = (le.MaleValue + le.FemaleValue) / 2,
                    Male = le.MaleValue,
                    Female = le.FemaleValue,
                    YearSpan = le.YearSpan
                });
        }

        private IQueryable<SchoolResultDTO> GetSchoolResults(string kommunId, int year)
        {
            return _context.SchoolResultsGradeNine
                .AsNoTracking()
                .Where(s => s.Kommun.Kommun == kommunId && s.EndYear == year)
                .GroupBy(s => s.Subject)
                .Select(sr => new SchoolResultDTO
                {
                    Subject = sr.Key,
                    AverageGrade = Math.Round((decimal)sr.Average(s => s.GradePoints), 1)
                });
        }

        private IQueryable<TopSchoolDTO> GetTopSchools(string kommunId, int year)
        {
            return _context.SchoolResultsGradeNine
                .AsNoTracking()
                .Where(s => s.Kommun.Kommun == kommunId && s.EndYear == year)
                .GroupBy(s => s.SchoolName)
                .Select(ts => new TopSchoolDTO
                {
                    SchoolName = ts.Key,
                    AverageGrade = Math.Round((decimal)ts.Average(s => s.GradePoints), 1)
                })
                .OrderByDescending(s => s.AverageGrade)
                .Take(3);
        }
    }
}
