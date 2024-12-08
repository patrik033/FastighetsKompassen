using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class KpiDTO
    {
        public int? TotalSales { get; set; }
        public decimal? MiddleAge { get; set; }
        public int? TotalCrimes { get; set; }
        public decimal? AvgIncome { get; set; }
    }
}
