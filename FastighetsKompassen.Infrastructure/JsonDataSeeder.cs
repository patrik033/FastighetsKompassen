using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FastighetsKompassen.Infrastructure
{
    public class JsonDataSeeder
    {
        private readonly AppDbContext _dbContext;

        public JsonDataSeeder(AppDbContext dbContext)
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

            // Kontrollera om kommunen redan finns
            var exists = await _dbContext.Kommuner
              .AsNoTracking()
                .AnyAsync(k => k.Kommun == kommunData.Kommun && k.Kommunnamn == kommunData.Kommunnamn);

            if (exists)
            {
                return false; // Kommunen finns redan
            }

            // Lägg till ny kommun och dess relaterade data
            _dbContext.Kommuner.Add(kommunData);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
