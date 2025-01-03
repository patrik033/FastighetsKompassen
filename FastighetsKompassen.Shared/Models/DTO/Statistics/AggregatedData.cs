using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO.Statistics
{
    public class AggregatedData
    {
        public decimal? TotalScore { get; set; }
        public string Kommun { get; set; }
    }
}
