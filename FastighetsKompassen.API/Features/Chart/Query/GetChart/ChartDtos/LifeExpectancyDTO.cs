using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart.ChartDtos
{
    public class LifeExpectancyDTO
    {
        public decimal Total { get; set; }
        public decimal Male { get; set; }
        public decimal Female { get; set; }
        public string YearSpan { get; set; }
    }
}
