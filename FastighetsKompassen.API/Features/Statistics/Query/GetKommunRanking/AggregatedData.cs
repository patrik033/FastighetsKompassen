using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Statistics.GetKommunRanking
{
    public class AggregatedData
    {
        public decimal? TotalScore { get; set; }
        public string Kommun { get; set; }
    }
}
