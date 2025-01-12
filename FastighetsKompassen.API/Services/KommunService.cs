using System.Text.Json;
using EFCore.BulkExtensions;
using FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity;
using FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
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
}
