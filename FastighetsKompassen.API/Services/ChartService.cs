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


        public async Task<object> GetRealEstateChartData(string kommunId)
        {
            var data = await _context.RealEstateYearlySummary
                .Where(r => r.Kommun.Kommun == kommunId && r.Year == DateTime.Now.Year)
                .GroupBy(r => r.PropertyType)
                .Select(g => new
                {
                    PropertyType = g.Key,
                    SalesCount = g.Sum(r => r.SalesCount)
                })
                .ToListAsync();

            return data;
        }

        public async Task<object> GetCrimeChartData(string kommunId)
        {
            var data = await _context.PoliceEventSummary
                .Where(p => p.Kommun.Kommun == kommunId && p.Year == DateTime.Now.Year)
                .GroupBy(p => p.EventType)
                .Select(g => new
                {
                    EventType = g.Key,
                    EventCount = g.Sum(p => p.EventCount)
                })
                .ToListAsync();

            return data;
        }

        public async Task<object> GetIncomeChartData(string kommunId)
        {
            var data = await _context.Income
                .Where(i => i.Kommun.Kommun == kommunId)
                .OrderBy(i => i.Year)
                .Select(i => new { i.Year, i.MiddleValue })
                .ToListAsync();

            return data;
        }

        public async Task<object> GetLifeExpectancyChartData(string kommunId)
        {
            var data = await _context.AverageLifeTime
                .Where(l => l.Kommun.Kommun == kommunId)
                .Select(l => new
                {
                    Male = l.MaleValue,
                    Female = l.FemaleValue
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<object> GetSchoolResultsChartData(string kommunId)
        {
            var data = await _context.SchoolResultsGradeNine
                .Where(s => s.Kommun.Kommun == kommunId)
                .GroupBy(s => s.Subject)
                .Select(g => new
                {
                    Subject = g.Key,
                    AvgGradePoints = g.Average(s => s.GradePoints)
                })
                .ToListAsync();

            return data;
        }
    }
}
