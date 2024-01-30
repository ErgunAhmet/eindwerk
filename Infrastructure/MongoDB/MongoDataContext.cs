using MongoDB.Bson;
using MongoDB.Driver;
using Shared.MongoDB;
using Shared.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public class MongoDataContext<T> : IDisposable
    {
        private MongoClient _mongoClient;
        private IMongoCollection<T> _mongoCollection;

        private readonly string _databaseName;
        public string CollectionName { get; private set; }

        public MongoDataContext(string connectionString, string collectionName)
        {
            var mongoUrl = new MongoUrl(connectionString);
            _mongoClient = new MongoClient(mongoUrl);

            _databaseName = mongoUrl.DatabaseName;
            CollectionName = collectionName;
        }

        public async Task<IMongoCollection<T>> GetCollectionAsync()
        {
            if (_mongoCollection == null)
                _mongoCollection = _mongoClient.GetDatabase(_databaseName).GetCollection<T>(CollectionName);

            if (Scope<UnitOfWork>.Current != null && Scope<UnitOfWork>.Current.IsTransactional && Scope<UnitOfWork>.Current.TransactionalSession == null)
            {
                var session = await _mongoClient.StartSessionAsync();
                Scope<UnitOfWork>.Current.TransactionalSession = new MongoDBSession(session);
            }

            return _mongoCollection;
        }

        public async Task CreateCollectionIfNotExistsAsync()
        {
            var mongoDatabase = _mongoClient.GetDatabase(_databaseName);

            var filter = new BsonDocument("name", CollectionName);
            var collections = await mongoDatabase.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });

            if (!await collections.AnyAsync())
                await mongoDatabase.CreateCollectionAsync(CollectionName);
        }

        public IClientSessionHandle GetSession()
        {
            if (Scope<UnitOfWork>.Current?.TransactionalSession == null)
                return null;

            return ((MongoDBSession)Scope<UnitOfWork>.Current.TransactionalSession).Session;
        }

        public void Dispose()
        {
            _mongoClient = null;
            _mongoCollection = null;
        }
    }
}
