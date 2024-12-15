using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.DTO
{
    public class ChartDataDTO
    {
        public List<PropertySalesDTO> PropertySales { get; set; }
        public List<CrimeDistributionDTO> CrimeDistribution { get; set; }
        public List<IncomeDTO> AvgIncome { get; set; }
        public LifeExpectancyDTO AvgLifeExpectancy { get; set; }
        public List<SchoolResultDTO> SchoolResultYearNine { get; set; }
        public List<TopSchoolDTO> TopSchools { get; set; }
    }
}
