using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Statistics.GetKommunRanking
{
    public class KommunRankingDto
    {
        public string Kommun { get; set; }
        public string KommunNamn { get; set; }
        public decimal? TotalScore { get; set; }
        public decimal? ScoreChange { get; set; }
    }
}
