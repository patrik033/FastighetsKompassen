using FastighetsKompassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Services
{
    public class BackupService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _backupPath;

        public BackupService(AppDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _backupPath = configuration["BackupDirectory"] ?? "Backups";
            Directory.CreateDirectory(_backupPath);
        }

        public async Task<string> CreateBackupAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var backupFileName = $"Backup_{timestamp}.bak";
            var backupFilePath = Path.Combine(_backupPath, backupFileName);

            var databaseName = _dbContext.Database.GetDbConnection().Database;
            var sqlCommand = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFilePath}'";

            using var connection = _dbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = sqlCommand;
            await command.ExecuteNonQueryAsync();

            return backupFilePath;
        }
    }
}
