using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.MapData
{
    public class MapFeatures
    {
        public int Id { get; set; } // Primärnyckel
        public string Type { get; set; }
        public MapGeometry Geometry { get; set; }
        public MapProperties Properties { get; set; }
    }
}
