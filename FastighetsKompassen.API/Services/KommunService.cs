using System.Text.Json;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.Infrastructure.Services;

public class KommunService
{
    private readonly AppDbContext _dbContext;

    public KommunService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddKommunFromJsonAsync(Stream jsonStream)
    {
        using var bufferedStream = new BufferedStream(jsonStream, 8192);

        var kommunData = await JsonSerializer.DeserializeAsync<KommunData>(bufferedStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (kommunData == null)
        {
            throw new InvalidOperationException("JSON-filen innehåller ogiltig data.");
        }

        var exists = await _dbContext.Kommuner.AnyAsync(k =>
            k.Kommun == kommunData.Kommun && k.Kommunnamn == kommunData.Kommunnamn);

        if (exists)
        {
            return false;
        }

        _dbContext.Kommuner.Add(kommunData);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Result> DeleteKommunAsync(int kommunId)
    {
        var kommun = await _dbContext.Kommuner
            .FirstOrDefaultAsync(x => x.Id == kommunId);
      
            //.Include(k => k.RealEstateDataList
            //.Include(k => k.SchoolResultsForGrade6)
            //.Include(k => k.SchoolResultsForGrade9)
            ////.Include(k => k.SchoolResultsForGymnasium)
            //.FirstOrDefaultAsync(k => k.Id == kommunId);

        if (kommun == null)
        {
            return Result.Failure("Kommunen finns inte.");
        }

        _dbContext.Kommuner.Remove(kommun);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
