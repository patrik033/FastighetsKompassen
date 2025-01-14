using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Chart.Query.GetChart.ChartDtos
{
    public class CrimeDistributionDTO
    {
        public string? EventType { get; set; }
        public int? EventCount { get; set; }
    }
}
