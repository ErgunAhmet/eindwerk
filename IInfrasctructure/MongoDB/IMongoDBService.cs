using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IInfrasctructure.MongoDB
{
    public interface IMongoDBService
    {
        Task SetupAsync(params Assembly[] assembliesToScan);
    }
}
