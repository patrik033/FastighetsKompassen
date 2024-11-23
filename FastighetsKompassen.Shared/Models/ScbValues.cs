
using FastighetsKompassen.Shared.Models.SkolData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models
{
    public class ScbValues
    {
        public int Id { get; set; }
        public string IncomeComponent { get; set; }
        public string Sex { get; set; }
        public string Year { get; set; }
        public decimal MiddleValue { get; set; }
        public EducationLevelData? EducationData { get; set; }

        public int KommunDataId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
