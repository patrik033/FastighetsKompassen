using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class ComparisonRequestDTO
    {
        public string Municipality1 { get; set; } = string.Empty;
        public string Municipality2 { get; set; } = string.Empty;
        public List<ComparisonParameter> Parameters { get; set; } = new List<ComparisonParameter>();
    }
}
