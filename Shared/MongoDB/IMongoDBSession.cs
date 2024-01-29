using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MongoDB
{
    public interface IMongoDBSession : IDisposable
    {
        Task CommitTransactionAsync();
        Task AbortTransactionAsync();
    }
}
