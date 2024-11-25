using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.SkolData
{
    public class SchoolResultGradeSix
    {
        public int Id { get; set; }
        public string Subject { get; set; }  // Nytt fält för att markera vilket ämne datan tillhör
        public string EducationLevel { get; set; } // Nytt fält för utbildningsnivå (Årskurs 6, 9 eller Gymnasium)
                                                   //skola
        public int StartYear { get; set; }
        public string SchoolName { get; set; }
        //skol-enhetskod
        public int SchoolUnitCode { get; set; }
        //skolkummun
        public string SchoolMunicipality { get; set; }

        //saknas(kommunkod)
        public double? MunicipalityCode { get; set; }

        //typ av huvudman
        public string HeadOrganizationType { get; set; }
        //huvudman
        public string HeadOrganizationName { get; set; }

        //(saknas) - huvudmanorgnr
        public double? HeadOrganizationNumber { get; set; }
        public string SubTest { get; set; }  // "Delprov"
        public string? TestCode { get; set; }
        public double? TotalParticipation { get; set; } // Som deltagit - Totalt
        public double? FemaleParticipation { get; set; } // Som deltagit - Flickor
        public double? MaleParticipation { get; set; } // Som deltagit - Pojkar
        public double? TotalGradeAF { get; set; } // Provbetyg A-F Totalt
        public double? FemaleGradeAF { get; set; } // Provbetyg A-F Flickor
        public double? MaleGradeAF { get; set; } // Provbetyg A-F Pojkar
        public double? TotalGradeAE { get; set; } // Provbetyg A-E Totalt
        public double? FemaleGradeAE { get; set; } // Provbetyg A-E Flickor
        public double? MaleGradeAE { get; set; } // Provbetyg A-E Pojkar
        public double? GradePoints { get; set; } // Provbetygspoäng Totalt
        public double? FemaleGradePoints { get; set; } // Provbetygspoäng Flickor
        public double? MaleGradePoints { get; set; } // Provbetygspoäng Pojkar

        public int KommunId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
