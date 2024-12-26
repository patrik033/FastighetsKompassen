using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.MapData
{
    public class MapProperties
    {
        public int Id { get; set; } // Primärnyckel
        public string Name { get; set; }
        public int OsmId { get; set; }
        public MapTags Tags { get; set; }
        public List<MapFeatures> Municipalities { get; set; } // Navigering till kommuner
    }
}
