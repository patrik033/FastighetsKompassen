using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.MapData
{
    public class MapTags
    {
        public int Id { get; set; } // Primärnyckel
        public string Ref { get; set; }
        public string NameSv { get; set; }
        public string Population { get; set; }
        public string Boundary { get; set; }
        public string Wikidata { get; set; }
        public string Wikipedia { get; set; }
        public string RefScb { get; set; }
        public string ShortName { get; set; }
    }
}
