using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.RealEstate
{
    public class RealEstateYearlySummary
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string? PropertyType { get; set; }
        public int SalesCount { get; set; }
        public decimal? TotalSalesAmount { get; set; }
    }
}
