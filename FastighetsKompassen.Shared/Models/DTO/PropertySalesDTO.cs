using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class PropertySalesDTO
    {
        public string? PropertyType { get; set; }
        public int ?SalesCount { get; set; }
        public decimal? TotalSalesAmount { get; set; }
    }
}
