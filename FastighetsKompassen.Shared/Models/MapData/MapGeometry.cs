using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.MapData
{
    public class MapGeometry
    {
        public int Id { get; set; } // Primärnyckel
        public string Type { get; set; }
        public string Coordinates { get; set; }
    }
}
