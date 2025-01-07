using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.API.Features.Statistics.GetKommunRanking
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } // Datan för den aktuella sidan
        public int TotalCount { get; set; } // Totalt antal poster
        public int TotalPages { get; set; } // Totalt antal sidor
        public int CurrentPage { get; set; } // Nuvarande sida
    }
}
