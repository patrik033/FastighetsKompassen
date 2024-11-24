using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.SkolData;

namespace FastighetsKompassen.API.Services
{
    public class SchoolService
    {
        private readonly AppDbContext _dbContext;

        public SchoolService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSchoolDataAsync(List<SchoolResultGradeSix> gradeSixData, List<SchoolResultGradeNine> gradeNineData)
        {
            if (gradeSixData == null && gradeNineData == null)
            {
                throw new ArgumentException("Ingen skoldata angavs.");
            }

            if (gradeSixData != null && gradeSixData.Any())
            {
                _dbContext.SchoolResultsGradeSix.AddRange(gradeSixData);
            }

            if (gradeNineData != null && gradeNineData.Any())
            {
                _dbContext.SchoolResultsGradeNine.AddRange(gradeNineData);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
