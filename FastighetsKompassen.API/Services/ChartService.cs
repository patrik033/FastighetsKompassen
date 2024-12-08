using FastighetsKompassen.Infrastructure.Data;

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


        public async Task<object> GetChartData(string kommunId)
        {
            var latestYear = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId)
                .MaxAsync(s => s.StartYear);

            var schoolResult = await _context.SchoolResultsGradeNine
                .AsNoTracking()
                 .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == latestYear)
                 .GroupBy(s => s.Subject)
                 .Select(subGroup => new
                 {
                     Subject = subGroup.Key,
                     AverageGrade = Math.Round((decimal)subGroup.Average(s => s.GradePoints), 1)
                 })
                 .ToListAsync();

            var topSchools = await _context.SchoolResultsGradeNine
                .AsNoTracking()
                 .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == latestYear)
                 .GroupBy(s => s.SchoolName)
                 .Select(schoolGroup => new
                 {
                     SchoolName = schoolGroup.Key,
                     AverageGrade = Math.Round((decimal)schoolGroup.Average(s => s.GradePoints), 1)
                 })
                 .OrderByDescending(s => s.AverageGrade)
                 .Take(3)
                 .ToListAsync();

            var totalSales = await _context.RealEstateYearlySummary
                .AsNoTracking()
                .Where(r => r.Kommun.Kommun == kommunId &&
                (r.PropertyType == "Villa" ||
                 r.PropertyType == "Lägenhet" ||
                 r.PropertyType == "Radhus"))
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new
                {
                    PropertySales = g.Select(r => new
                    {
                        r.PropertyType,
                        r.SalesCount,
                        r.TotalSalesAmount
                    }),
                }).FirstOrDefaultAsync();


            var totalCrimes = await _context.PoliceEventSummary
                .AsNoTracking()
                .Where(p => p.Kommun.Kommun == kommunId)
                .GroupBy(p => p.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new
                {
                    CrimeDistribution = g.Select(e => new
                    {
                        e.EventType,
                        e.EventCount
                    })
                    .Take(5)
                }).FirstOrDefaultAsync();


            var lifeExpectancy = await _context.AverageLifeTime
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Kommun.Kommun == kommunId);

            var avgIncome = await _context.Income
                .AsNoTracking()
               //240 == totalt
               .Where(i => i.Kommun.Kommun == kommunId && i.IncomeComponent == "240")
               .GroupBy(i => i.Year)
               .OrderByDescending(g => g.Key)
               .Select(g => new
               {
                   Income = g.Average(e => e.MiddleValue),
                   Year = g.Key
               })

               .Take(5)
               .ToListAsync();


            return new
            {
                AvgIncome = avgIncome,
                TotalSales = totalSales,
                TotalCrimes = totalCrimes,
                AvgLifeExpectancy = new
                {
                    Total = (lifeExpectancy.MaleValue + lifeExpectancy.FemaleValue) / 2,
                    MaleEverage = lifeExpectancy.MaleValue,
                    FemaleAverage = lifeExpectancy.FemaleValue,
                    Yearspan = lifeExpectancy.YearSpan
                },
                SchoolResultYearNine = schoolResult,
                TopSchools = topSchools,
            };
        }
    }
}
