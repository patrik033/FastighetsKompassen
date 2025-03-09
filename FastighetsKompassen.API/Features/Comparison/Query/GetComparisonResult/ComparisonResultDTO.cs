using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{
    public class ComparisonResultDTO
    {
        public string Parameter { get; set; } = string.Empty; // Display name for the comparison parameter
        public string Municipality1 { get; set; } = string.Empty;
        public string Municipality2 { get; set; } = string.Empty;
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public decimal Difference { get; set; }
        public decimal PercentageDifference { get; set; }
        public decimal TimesLarger { get; set; }
        public string? FieldName { get; set; } // Additional field to display for School Results or Property Sales
    }
}
