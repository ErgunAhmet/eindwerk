using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IInfrasctructure.MongoDB
{
    public interface IMongoDBBaseRepository : IDisposable
    {
        Task SetupAsync(string connectionString);
    }
}
