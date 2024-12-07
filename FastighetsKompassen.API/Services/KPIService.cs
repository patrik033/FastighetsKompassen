using FastighetsKompassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Services
{
    public class KPIService
    {
        private readonly AppDbContext _context;

        public KPIService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetKPIAsync(string kommunId)
        {
            var latestYear = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId)
                .MaxAsync(s => s.StartYear);

            var schoolResult = await _context.SchoolResultsGradeNine
                 .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == latestYear)
                 .GroupBy(s => s.Subject)
                 .Select(subGroup => new
                 {
                     Subject = subGroup.Key,
                     AverageGrade = Math.Round((decimal)subGroup.Average(s => s.GradePoints),1)
                 })
                 .ToListAsync();

            var topSchools = await _context.SchoolResultsGradeNine
                 .Where(s => s.Kommun.Kommun == kommunId && s.StartYear == latestYear)
                 .GroupBy(s => s.SchoolName)
                 .Select(schoolGroup => new
                 {
                     SchoolName = schoolGroup.Key,
                     AverageGrade = Math.Round((decimal)schoolGroup.Average(s => s.GradePoints),1)
                 })
                 .OrderByDescending(s => s.AverageGrade)
                 .Take(3)
                 .ToListAsync();



            var totalSales = await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId)
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new
                {
                    Sales = g.Sum(e => e.SalesCount),
                    PropertySales = g.Select(r => new
                    {
                        r.PropertyType,
                        r.SalesCount
                    }),
                }).FirstOrDefaultAsync();



            var totalCrimes = await _context.PoliceEventSummary
                .Where(p => p.Kommun.Kommun == kommunId)
                .GroupBy(p => p.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new
                {
                    Crimes = g.Sum(c => c.EventCount),
                    CrimeDistribution = g.Select(e => new
                    {
                        e.EventType,
                        e.EventCount
                    })
                }).FirstOrDefaultAsync();




            var lifeExpectancy = await _context.AverageLifeTime
                .FirstOrDefaultAsync(l => l.Kommun.Kommun == kommunId);

            var middleAge = await _context.AverageMiddleAge
                .Where(m => m.Kommun.Kommun == kommunId)
                .GroupBy (m => m.Year)
                .OrderByDescending (g => g.Key)
                .Select(d => d.Select(e => e.Total))
                //.Select(d => new
                //{
                //    TotalValue = d.Select(e => new
                //    {
                //        e.Total
                //    })
                //})
                .FirstOrDefaultAsync();

            var avgIncome = await _context.Income
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
                TotalSales = totalSales,
                TotalCrimes = totalCrimes,
                AvgIncome = avgIncome,
                MiddleAge = middleAge,
                AvgLifeExpectancy = new
                {

                    Total = (lifeExpectancy.MaleValue + lifeExpectancy.FemaleValue) / 2,
                    MaleEverage = lifeExpectancy.MaleValue,
                    FemaleAverage = lifeExpectancy.FemaleValue,
                    Yearspan = lifeExpectancy.YearSpan
                },
                SchoolResultYearNine = schoolResult,
                TopSchools = topSchools
            };


        }
    }
}
