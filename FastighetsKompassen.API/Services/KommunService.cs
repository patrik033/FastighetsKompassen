using System.Text.Json;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models;
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
        var kommunData = await JsonSerializer.DeserializeAsync<KommunData>(jsonStream, new JsonSerializerOptions
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
}
