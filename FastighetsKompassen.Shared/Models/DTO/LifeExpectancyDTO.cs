using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class LifeExpectancyDTO
    {
        public decimal Total { get; set; }
        public decimal Male { get; set; }
        public decimal Female { get; set; }
        public string YearSpan { get; set; }
    }
}
