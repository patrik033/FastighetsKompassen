using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.RealEstate;

namespace FastighetsKompassen.API.Services
{
    public class RealEstateService
    {
        private readonly AppDbContext _dbContext;

        public RealEstateService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRealEstateDataAsync(List<RealEstateData> realEstateData)
        {
            if (realEstateData == null || !realEstateData.Any())
            {
                throw new ArgumentException("Listan med fastighetsdata är tom eller ogiltig.");
            }

            _dbContext.RealEstateData.AddRange(realEstateData);
            await _dbContext.SaveChangesAsync();
        }
    }
}
