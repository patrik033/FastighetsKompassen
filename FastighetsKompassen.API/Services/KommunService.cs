using System.Text.Json;
using EFCore.BulkExtensions;
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

    public async Task<KommunData?> ParseKommunFromJsonAsync(Stream jsonStream)
    {
        try
        {
            using var bufferedStream = new BufferedStream(jsonStream, 8192);
            var kommunData = await JsonSerializer.DeserializeAsync<KommunData>(bufferedStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return kommunData;
        }
        catch
        {
            return null; // Returnera null om parsing misslyckas
        }
    }

    public async Task AddKommunerInBulkWithRelatedDataAsync(List<KommunData> kommuner)
    {
        var existingKommuner = await _dbContext.Kommuner
            .Select(k => new { k.Kommun, k.Kommunnamn })
            .ToListAsync();

        var kommunerToInsert = kommuner
            .Where(k => !existingKommuner.Any(e => e.Kommun == k.Kommun && e.Kommunnamn == k.Kommunnamn))
            .ToList();

        if (kommunerToInsert.Any())
        {
            // Spara kommuner
            await _dbContext.BulkInsertAsync(kommunerToInsert);

            // Förbered relaterad data
            var realEstateList = kommunerToInsert
                .SelectMany(k => k.RealEstateDataList.Select(r => { r.KommunDataId = k.Id; return r; }))
                .ToList();

            var policeEventsList = kommunerToInsert
                .SelectMany(k => k.PoliceEvents.Select(p => { p.KommunDataId = k.Id; return p; }))
                .ToList();

            var grade6Results = kommunerToInsert
                .SelectMany(k => k.SchoolResultsForGrade6.Select(g6 => { g6.KommunId = k.Id; return g6; }))
                .ToList();

            var grade9Results = kommunerToInsert
                .SelectMany(k => k.SchoolResultsForGrade9.Select(g9 => { g9.KommunId = k.Id; return g9; }))
                .ToList();

            // Spara relaterad data
            await _dbContext.BulkInsertAsync(realEstateList);
            await _dbContext.BulkInsertAsync(policeEventsList);
            await _dbContext.BulkInsertAsync(grade6Results);
            await _dbContext.BulkInsertAsync(grade9Results);
        }
    }

    public async Task AddKommunerInBulkAsync(List<KommunData> kommuner)
    {
        // Hämta existerande kommuner för att undvika duplicering
        var existingKommuner = await _dbContext.Kommuner
            .Select(k => new { k.Kommun, k.Kommunnamn })
            .ToListAsync();

        var kommunerToInsert = kommuner
            .Where(k => !existingKommuner.Any(e => e.Kommun == k.Kommun && e.Kommunnamn == k.Kommunnamn))
            .ToList();

        if (kommunerToInsert.Any())
        {
            // Använd BulkInsert för snabb inmatning
            await _dbContext.BulkInsertAsync(kommunerToInsert);
        }
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

        if (kommun == null)
        {
            return Result.Failure("Kommunen finns inte.");
        }

        _dbContext.Kommuner.Remove(kommun);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
