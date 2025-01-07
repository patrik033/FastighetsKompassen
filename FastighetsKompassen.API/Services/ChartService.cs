using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FastighetsKompassen.API.Services
{
    public class ChartService
    {
        private readonly AppDbContext _context;
        public ChartService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ChartDataDTO> GetChartData(string kommunId)
        {
            var latestYear = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId)
                .MaxAsync(s => s.EndYear);

            var result = new ChartDataDTO
            {
                PropertySales = await GetPropertySales(kommunId),
                CrimeDistribution = await GetCrimeDistribution(kommunId, latestYear).Take(5).ToListAsync(),
                AvgIncome = await GetIncomeData(kommunId).Take(5).ToListAsync(),
                AvgLifeExpectancy = await GetLifeExpectancy(kommunId).FirstOrDefaultAsync(),
                SchoolResultYearNine = await GetSchoolResults(kommunId, latestYear).ToListAsync(),
                TopSchools = await GetTopSchools(kommunId, latestYear).ToListAsync()
            };

            return result;
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
