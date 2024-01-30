using MongoDB.Driver;
using Shared.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public class MongoDBSession : IMongoDBSession
    {
        public IClientSessionHandle Session { get; private set; }

        public MongoDBSession(IClientSessionHandle session)
        {
            session.StartTransaction();
            Session = session;
        }

        public async Task CommitTransactionAsync()
        {
            await Session.CommitTransactionAsync();
        }

        public async Task AbortTransactionAsync()
        {
            await Session.AbortTransactionAsync();
        }

        public void Dispose()
        {
            if (Session?.IsInTransaction == true)
                Session?.AbortTransaction();

            Session?.Dispose();
            Session = null;
        }
    }
}
