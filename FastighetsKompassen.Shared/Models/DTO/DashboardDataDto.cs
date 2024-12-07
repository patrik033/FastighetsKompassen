using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class DashboardDataDto
    {
        public DashboardKpiDto KPI { get; set; }
        public DashboardChartsDto Charts { get; set; }
    }

    public class DashboardKpiDto
    {
        public int TotalSales { get; set; }
        public decimal AvgLifeExpectancyMale { get; set; }
        public decimal AvgLifeExpectancyFemale { get; set; }
        public decimal AvgIncome { get; set; }
        public int TotalCrimes { get; set; }
    }

    public class DashboardChartsDto
    {
        public IEnumerable<PropertySalesDto> PropertySales { get; set; }
        public IEnumerable<CrimeDistributionDto> CrimeDistribution { get; set; }
        public IEnumerable<AvgPriceDto> AvgPrice { get; set; }
        public IEnumerable<IncomeOverYearsDto> IncomeOverYears { get; set; }
        public LifeExpectancyDto LifeExpectancy { get; set; }
        public IEnumerable<SchoolResultsDto> SchoolResults { get; set; }
        public IEnumerable<TopSchoolsDto> TopSchools { get; set; }
    }

    public class PropertySalesDto
    {
        public string? PropertyType { get; set; }
        public int? SalesCount { get; set; }
    }

    public class CrimeDistributionDto
    {
        public string? EventType { get; set; }
        public int? EventCount { get; set; }
    }

    public class AvgPriceDto
    {
        public string? PropertyType { get; set; }
        public decimal? AvgPrice { get; set; }
    }

    public class IncomeOverYearsDto
    {
        public string? Year { get; set; }
        public decimal MiddleValue { get; set; }
    }

    public class LifeExpectancyDto
    {
        public decimal Male { get; set; }
        public decimal Female { get; set; }
    }

    public class SchoolResultsDto
    {
        public string? Subject { get; set; }
        public double? AvgGradePoints { get; set; }
    }

    public class TopSchoolsDto
    {
        public string? SchoolName { get; set; }
        public double? GradePoints { get; set; }
    }

}
