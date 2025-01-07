using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.PoliceData
{
    public class PoliceEvent
    {
        public int Id { get; set; }
        public string? PoliceOriginalId { get; set; }
        public DateTime? Datetime { get; set; }
        public string? Summary { get; set; }
        public string? Type { get; set; }
        public string? Body { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public Location? Location { get; set; }
        public int KommunDataId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
