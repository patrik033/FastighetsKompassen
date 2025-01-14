using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart.ChartDtos
{
    public class IncomeDTO
    {
        public int Year { get; set; }
        public decimal AvgIncome { get; set; }
    }
}
