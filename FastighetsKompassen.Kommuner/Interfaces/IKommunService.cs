using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Kommuner.Interfaces
{
    public interface IKommunService
    {
        Task<bool> AddKommunFromJsonAsync(Stream jsonStream);
    }
}
