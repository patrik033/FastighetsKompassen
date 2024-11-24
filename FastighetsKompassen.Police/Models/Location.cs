using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Police.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int PoliceEventId { get; set; }
        public PoliceEvent? PoliceEvent { get; set; }
    }
}
