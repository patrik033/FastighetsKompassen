using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO.Statistics
{
    public class KommunTrendDto
    {
        public int Year { get; set; }
        public decimal LifeTime { get; set; }
        public decimal AverageAge { get; set; }
        public decimal TotalSales { get; set; }
        public decimal SalesCount { get; set; }
        public decimal AvgSalesValue { get; set; }
        public Dictionary<string, decimal> Crimes { get; set; } = new Dictionary<string, decimal>();
        public decimal SchoolResultsYearNine { get; set; }
        public decimal SchoolResultsYearSix { get; set; }
    }
}
