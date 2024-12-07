using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models
{
    public class AverageLifeTime
    {
        public int Id { get; set; }
        public decimal MaleValue { get; set; }  // Medellivslängd för män
        public decimal FemaleValue { get; set; } // Medellivslängd för kvinnor
        public string YearSpan { get; set; }
        public int KommunDataId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
