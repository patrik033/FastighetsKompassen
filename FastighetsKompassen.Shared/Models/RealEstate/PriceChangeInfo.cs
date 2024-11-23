using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.RealEstate
{
    public class PriceChangeInfo
    {
        public int Id { get; set; }
        public bool Plus { get; set; }
        public bool Minus { get; set; }
        public int Value { get; set; }
    }
}
