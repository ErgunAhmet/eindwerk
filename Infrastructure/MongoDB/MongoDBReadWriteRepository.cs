using Domain.Entities.Base;
using IInfrasctructure.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public abstract class MongoDBReadWriteRepository<T> : MongoDBReadRepository<T>, IMongoDBReadWriteRepository<T> where T : class, IEntity
    {
        private const int MaxBulkInsert = 180;

        public virtual async Task InsertAsync(T obj)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                await collection.InsertOneAsync(obj);
            else
                await collection.InsertOneAsync(session, obj);
        }

        public virtual async Task BatchInsertAsync(IEnumerable<T> obj)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var data = obj?.ToList();

            while (data?.Any() == true)
            {
                List<T> processData;

                if (data.Count > MaxBulkInsert)
                    processData = data.Take(MaxBulkInsert).ToList();
                else
                    processData = data;

                var options = new InsertManyOptions { IsOrdered = false };

                if (session == null)
                    await collection.InsertManyAsync(processData, options);
                else
                    await collection.InsertManyAsync(session, processData, options);

                data.RemoveRange(0, processData.Count);
            }
        }

        public virtual async Task RemoveAsync(T obj)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            DeleteResult result = null;

            Expression<Func<T, bool>> match = x => x.Id == obj.Id;

            if (session == null)
                result = await collection.DeleteOneAsync(match);
            else
                result = await collection.DeleteManyAsync(session, match);

            if (result?.IsAcknowledged != true || result.DeletedCount == 0)
                throw new Exception($"Something went wrong with {nameof(MongoDBReadWriteRepository<T>.RemoveAllAsync)}: {nameof(DeleteResult.IsAcknowledged)} = {result?.IsAcknowledged.ToString()} and {nameof(DeleteResult.DeletedCount)} = {result?.DeletedCount.ToString()}");
        }

        public virtual async Task RemoveAllAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            DeleteResult result = null;

            if (session == null)
                result = await collection.DeleteManyAsync(match);
            else
                result = await collection.DeleteManyAsync(session, match);

            if (result?.IsAcknowledged != true)
                throw new Exception($"Something went wrong with {nameof(MongoDBReadWriteRepository<T>.RemoveAllAsync)}: {nameof(DeleteResult.IsAcknowledged)} = {result?.IsAcknowledged.ToString()} and {nameof(DeleteResult.DeletedCount)} = {result?.DeletedCount.ToString()}");
        }

        public virtual async Task UpdateAsync(T obj)
        {
            Expression<Func<T, bool>> match = x => x.Id == obj.Id;

            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            ReplaceOneResult result = null;

            if (session == null)
                result = await collection.ReplaceOneAsync(match, obj);
            else
                result = await collection.ReplaceOneAsync(session, match, obj);

            if (result?.IsAcknowledged != true || result.MatchedCount == 0)
                throw new Exception($"Something went wrong with {nameof(MongoDBReadWriteRepository<T>.UpdateAsync)}: {nameof(ReplaceOneResult.IsAcknowledged)} = {result?.IsAcknowledged.ToString()} and {nameof(ReplaceOneResult.MatchedCount)} = {result?.MatchedCount.ToString()}");
        }

        public virtual async Task BatchUpdateByIdAsync(IEnumerable<T> obj)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var bulkOps = new List<WriteModel<T>>();
            foreach (var record in obj)
            {
                var updatedOn = record.UpdatedOn;

                Expression<Func<T, bool>> match = x => record.Id == x.Id;

                var upsertOne = new ReplaceOneModel<T>(Builders<T>.Filter.Where(match), record)
                {
                    IsUpsert = false // no inserts for unmatched
                };
                bulkOps.Add(upsertOne);
            }

            BulkWriteResult<T> result;

            var options = new BulkWriteOptions { IsOrdered = false };

            if (session == null)
                result = await collection.BulkWriteAsync(bulkOps, options);
            else
                result = await collection.BulkWriteAsync(session, bulkOps, options);

            if (result?.IsAcknowledged != true || result.MatchedCount == 0)
                throw new Exception($"Something went wrong with {nameof(BatchUpdateByIdAsync)}: {nameof(BulkWriteResult.IsAcknowledged)} = {result?.IsAcknowledged.ToString()} and {nameof(BulkWriteResult.MatchedCount)} = {result?.MatchedCount.ToString()}");
        }

        protected virtual async Task UpdateSubCollectionItemAsync<TItem>(T item, Expression<Func<T, IEnumerable<TItem>>> field, Expression<Func<T, TItem>> updateField, TItem subItem) where TItem : SubEntity
        {
            var filter = Builders<T>.Filter.And(Builders<T>.Filter.Eq(x => x.Id, item.Id),
            Builders<T>.Filter.ElemMatch(field, x => x.Id == subItem.Id));

            var update = Builders<T>.Update.Set(updateField, subItem);

            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            UpdateResult result;

            if (session == null)
                result = await collection.UpdateOneAsync(filter, update);
            else
                result = await collection.UpdateOneAsync(session, filter, update);

            if (result?.IsAcknowledged != true || result.MatchedCount == 0)
                throw new Exception($"Something went wrong with {nameof(UpdateSubCollectionItemAsync)}: {nameof(BulkWriteResult.IsAcknowledged)} = {result?.IsAcknowledged.ToString()} and {nameof(BulkWriteResult.MatchedCount)} = {result?.MatchedCount.ToString()}");
        }
    }
}
