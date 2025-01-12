using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using FastighetsKompassen.Shared.Models;
using MediatR;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Upload.Command.UploadKommunFromFolder
{
    public class UploadKommunFromFolderHandler : IRequestHandler<UploadKommunFromFolderCommand, Result<List<FileProcessingResult>>>
    {
        private readonly AppDbContext _context;
        private readonly string _uploadsPath;

        public UploadKommunFromFolderHandler(AppDbContext context)
        {
            _context = context;
            _uploadsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
        }

        public async Task<Result<List<FileProcessingResult>>> Handle(UploadKommunFromFolderCommand request, CancellationToken cancellationToken)
        {
            var letter = request.Letter.ToUpper();

            // Kontrollera att mappen existerar
            var folderPath = Path.Combine(_uploadsPath, letter);
            if (!Directory.Exists(folderPath))
            {
                return Result<List<FileProcessingResult>>.Failure($"Mappen för bokstav '{letter}' kunde inte hittas.");
            }

            // Hämta filer
            var files = Directory.GetFiles(folderPath, "*.json");
            if (files.Length == 0)
            {
                return Result<List<FileProcessingResult>>.Failure($"Inga JSON-filer hittades i mappen för bokstav '{letter}'.");
            }

            var results = new List<FileProcessingResult>();
            foreach (var file in files)
            {
                try
                {
                    await using var stream = File.OpenRead(file);
                    var kommunData = await JsonSerializer.DeserializeAsync<KommunData>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (kommunData == null)
                    {
                        throw new InvalidOperationException("JSON-filen innehåller ogiltig data.");
                    }

                    var exists = await _context.Kommuner.AnyAsync(k =>
                        k.Kommun == kommunData.Kommun && k.Kommunnamn == kommunData.Kommunnamn);

                    if (!exists)
                    {
                        _context.Kommuner.Add(kommunData);
                        await _context.SaveChangesAsync();
                        results.Add(new FileProcessingResult(Path.GetFileName(file), "Lyckad"));
                    }
                    else
                    {
                        results.Add(new FileProcessingResult(Path.GetFileName(file), "Redan existerande"));
                    }
                }
                catch (Exception ex)
                {
                    results.Add(new FileProcessingResult(Path.GetFileName(file), $"Misslyckad: {ex.Message}"));
                }
            }

            return Result<List<FileProcessingResult>>.Success(results);
        }
    }
}
