using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Backup.Interfaces
{
    public interface IBackupService
    {
        Task<string> CreateBackupAsync();
    }
}
