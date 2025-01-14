using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends
{
    public class GetKommunTrendsHandler : IRequestHandler<GetKommunTrendsQuery,Result<List<KommunTrendDto>>>
    {
        private readonly AppDbContext _context;
        public GetKommunTrendsHandler(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Result<List<KommunTrendDto>>> Handle(GetKommunTrendsQuery request, CancellationToken cancellationToken)
        {
            var results = new List<KommunTrendDto>();

            foreach (var year in request.Year)
            {
                // Hämta livslängd
                var currentYearSpan = year >= 2019 ? "2019-2023" : "2014-2018";

                var lifeTime = await _context.AverageLifeTime
                    .Where(l => l.Kommun.Kommun == request.KommunId && l.YearSpan == currentYearSpan)
                    .AverageAsync(l => (decimal?)((l.MaleValue + l.FemaleValue) / 2)) ?? 0;

                // Hämta medelålder
                var averageAge = await _context.AverageMiddleAge
                    .Where(a => a.Kommun.Kommun == request.KommunId && a.Year == year)
                    .AverageAsync(a => (decimal?)a.Total) ?? 0;

                // Hämta fastighetsförsäljningar
                var realEstate = await _context.RealEstateYearlySummary
                    .Where(r => r.Kommun.Kommun == request.KommunId && r.Year == year)
                    .GroupBy(r => r.KommunId)
                    .Select(g => new
                    {
                        TotalSales = g.Sum(r => (decimal?)r.TotalSalesAmount) ?? 0,
                        SalesCount = g.Sum(r => (decimal?)r.SalesCount) ?? 0,
                        AvgSalesValue = g.Average(r => (decimal?)r.TotalSalesAmount / r.SalesCount) ?? 0
                    })
                    .FirstOrDefaultAsync();

                // Hämta brottsdata
                var crimeData = await _context.PoliceEventSummary
                    .Where(pe => pe.Kommun.Kommun == request.KommunId && pe.Year == year)
                    .GroupBy(pe => pe.EventType)
                    .Select(g => new { EventType = g.Key, Count = g.Sum(pe => (decimal?)pe.EventCount) ?? 0 })
                    .ToListAsync();

                // Hämta skolresultat (år 9 och 6)
                var schoolYearNine = await _context.SchoolResultsGradeNine
                    .Where(s => s.Kommun.Kommun == request.KommunId && s.EndYear == year)
                    .AverageAsync(s => (decimal?)s.GradePoints) ?? 0;

                var schoolYearSix = await _context.SchoolResultsGradeSix
                    .Where(s => s.Kommun.Kommun == request.KommunId && s.EndYear == year)
                    .AverageAsync(s => (decimal?)s.GradePoints) ?? 0;

                // Bygg upp resultat för detta år
                results.Add(new KommunTrendDto
                {
                    Year = year,
                    LifeTime = lifeTime > 0 ? lifeTime : (decimal?)null,
                    AverageAge = averageAge > 0 ? averageAge : (decimal?)null,
                    TotalSales = realEstate?.TotalSales,
                    SalesCount = realEstate?.SalesCount,
                    AvgSalesValue = realEstate?.AvgSalesValue,
                    Crimes = crimeData.ToDictionary(c => c.EventType, c => c.Count),
                    SchoolResultsYearNine = schoolYearNine > 0 ? schoolYearNine : (decimal?)null,
                    SchoolResultsYearSix = schoolYearSix > 0 ? schoolYearSix : (decimal?)null,
                });
            }

            var orderedResult = results.OrderByDescending(r => r.Year).ToList();

            if(orderedResult.Count > 0)
            {
                return Result<List<KommunTrendDto>>.Success(orderedResult);
            }
            return Result<List<KommunTrendDto>>.Failure("Ingen data returnerades, pröva en annan parameter");
        }
    }
}
