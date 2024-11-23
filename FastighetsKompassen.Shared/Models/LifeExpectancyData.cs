using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models
{
    public class LifeExpectancyData
    {
        public int Id { get; set; }
        public decimal Male { get; set; }  // Medellivslängd för män
        public decimal Female { get; set; } // Medellivslängd för kvinnor
        public decimal Total { get; set; }  // Medellivslängd totalt
        public int KommunDataId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
