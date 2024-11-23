using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.SkolData
{
    public class EducationLevelData
    {
        public int Id { get; set; }
        public int PreGymnasial { get; set; }
        public int Gymnasial { get; set; }
        public int PostGymnasialUnder3Years { get; set; }
        public int PostGymnasial3YearsOrMore { get; set; }
        public int MissingInfo { get; set; }

        public int KommunDataId { get; set; } // Främmande nyckel
        public KommunData? Kommun { get; set; } // Navigation tillbaka till KommunData
    }
}
