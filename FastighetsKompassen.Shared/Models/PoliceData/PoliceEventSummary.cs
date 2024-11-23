using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.PoliceData
{
    public class PoliceEventSummary
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string EventType { get; set; }
        public int EventCount { get; set; }
    }
}
