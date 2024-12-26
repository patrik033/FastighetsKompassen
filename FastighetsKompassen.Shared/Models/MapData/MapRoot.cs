using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.MapData
{
    public class MapRoot
    {
        public string Type { get; set; } // "FeatureCollection"
        public List<MapFeatures> Features { get; set; }
    }
}
