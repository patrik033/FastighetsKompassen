using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.PoliceData;

namespace FastighetsKompassen.API.Services
{
    public class PoliceService
    {
        private readonly AppDbContext _dbContext;

        public PoliceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPoliceEventsAsync(List<PoliceEvent> policeEvents)
        {
            if (policeEvents == null || !policeEvents.Any())
            {
                throw new ArgumentException("Listan med polisdata är tom eller ogiltig.");
            }

            _dbContext.PoliceEvents.AddRange(policeEvents);
            await _dbContext.SaveChangesAsync();
        }
    }
}
