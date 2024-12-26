using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO.Statistics
{
    public class KommunTrendsRequestDto
    {
        public string kommunId { get; set; }
        public int[] Year { get; set; }
    }
}
