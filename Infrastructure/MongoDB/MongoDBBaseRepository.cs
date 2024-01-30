using Domain.Entities.Base;
using IInfrasctructure.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public abstract class MongoDBBaseRepository<T> : IDisposable, IMongoDBBaseRepository where T : class, IEntity
    {
        protected const int PageSize = 50;
        public MongoDataContext<T> MongoDataContext { get; private set; }

        public virtual async Task SetupAsync(string connectionString)
        {
            MongoDataContext = new MongoDataContext<T>(connectionString, GetCollectionName());
            await MongoDataContext.CreateCollectionIfNotExistsAsync();
        }

        protected virtual string GetCollectionName()
        {
            return typeof(T).Name;
        }

        public void Dispose()
        {
            MongoDataContext?.Dispose();
            MongoDataContext = null;
        }
    }
}
