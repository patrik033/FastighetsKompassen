using FastighetsKompassen.Backup.Interfaces;
using FastighetsKompassen.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Backup.Services
{
    public class BackupService //: //IBackupService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _backupDirectory = "Backups";

        public BackupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            // Skapa backup-mappen om den inte finns
            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory);
            }
        }

        //public async Task<string> CreateBackupAsync()
        //{
        //    if (!Directory.Exists(_backupDirectory))
        //    {
        //        Directory.CreateDirectory(_backupDirectory);
        //    }

        //    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        //    var backupFileName = $"Backup_{timestamp}.bak";
        //    var backupFilePath = Path.Combine(_backupDirectory, backupFileName);

        //    var connectionString = _dbContext.Database.GetConnectionString();
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new InvalidOperationException("Kunde inte hitta en giltig anslutningssträng.");
        //    }

        //    // Utför backup med en SQL-kommandokörning
        //    using var connection = new SqlConnection(connectionString);
        //    await connection.OpenAsync();

        //    var sqlCommand = $@"
        //        BACKUP DATABASE [{connection.Database}]
        //        TO DISK = '{backupFilePath}'
        //    ";

        //    using var command = new SqlCommand(sqlCommand, connection);
        //    await command.ExecuteNonQueryAsync();

        //    return backupFilePath;
        //}
    }
}
