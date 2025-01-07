using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult
{ 
    public enum ComparisonParameter
    {
        SchoolResults = 0,
        PropertySales = 1,
        TotalSales = 2,
        TotalCrimes = 3,
        AvgIncome = 4,
        AvgLifeExpectancy = 5
    }
}
