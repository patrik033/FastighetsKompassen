
//using FastighetsKompassen.Shared.Models.PoliceData;
//using FastighetsKompassen.Shared.Models.RealEstate;
//using FastighetsKompassen.Shared.Models.SkolData;
using FastighetsKompassen.Shared.Models.PoliceData;
using FastighetsKompassen.Shared.Models.RealEstate;
using FastighetsKompassen.Shared.Models.SkolData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models
{
    public class KommunData
    {
        public int Id { get; set; }
        public string? Kommun { get; set; }
        public string? Kommunnamn { get; set; }

        //livslängs, inkomst och skolresultat
        public LifeExpectancyData? LifeExpectancy { get; set; }
        public ScbValues? Income { get; set; }



        public List<SkolData.SchoolResultGradeSix> SchoolResultsForGrade6 { get; set; } = new List<SchoolResultGradeSix>();  // Lista med skolor
        public List<SchoolResultGradeNine> SchoolResultsForGrade9 { get; set; } = new List<SchoolResultGradeNine>();  // Lista med skolor
        //public List<SchoolResultGymnasium> SchoolResultsForGymnasium { get; set; } = new List<SchoolResultGymnasium>();  // Lista med skolor
        public EducationLevelData? EducationData { get; set; }

        

        // Listor för att lagra alla polis- och fastighetshändelser
        public List<PoliceEvent> PoliceEvents { get; set; } = new List<PoliceEvent>(); // Ny lista för relaterade händelser
        // Sammanställning av polishändelser per typ och år
        public List<PoliceEventSummary> PoliceEventSummary { get; set; } = new List<PoliceEventSummary>();


        public List<RealEstateData> RealEstateDataList { get; set; } = new List<RealEstateData> { };
        // Sammanställning av fastighetsdata per år
        public List<RealEstateYearlySummary> RealEstateYearlySummary { get; set; } = new List<RealEstateYearlySummary>();

    }
}
