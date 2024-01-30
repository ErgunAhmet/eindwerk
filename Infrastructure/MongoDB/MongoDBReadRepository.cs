using Domain.Entities.Base;
using Domain.Other;
using Domain.Resources;
using IInfrasctructure.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public abstract class MongoDBReadRepository<T> : MongoDBBaseRepository<T>, IMongoDBReadRepository<T> where T : class, IEntity
    {
        public virtual async Task<List<T>> WhereAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.FindAsync<T>(match)).ToListAsync();
            else
                return await (await collection.FindAsync<T>(session, match)).ToListAsync();
        }

        public virtual async Task<Paged<T>> WherePagedAsync(Expression<Func<T, bool>> match, int page)
        {
            page = page > 0 ? page - 1 : 0;
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            Task<List<T>> itemsTask;
            var countTask = CountAsync(match);

            var pageSize = PageSize;

            if (session == null)
                itemsTask = collection.Find(match).Skip(page * pageSize).Limit(pageSize).ToListAsync();
            else
                itemsTask = collection.Find(session, match).Skip(page * pageSize).Limit(pageSize).ToListAsync();

            await Task.WhenAll(countTask, itemsTask);

            var items = itemsTask.Result;
            var count = countTask.Result;

            return new Paged<T>
            {
                Items = items,
                ItemsPerPage = pageSize,
                Page = page + 1,
                Pages = (int)Math.Ceiling((double)count / pageSize),
                TotalItems = count,
            };
        }

        public virtual async Task<Paged<TInherited>> WherePagedAsync<TInherited>(Expression<Func<TInherited, bool>> match, int page)
            where TInherited : T
        {
            page = page > 0 ? page - 1 : 0;
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            Task<List<TInherited>> itemsTask;
            var countTask = CountAsync<TInherited>(match);

            var pageSize = PageSize;

            if (session == null)
                itemsTask = collection.OfType<TInherited>().Find(match).Skip(page * pageSize).Limit(pageSize).ToListAsync();
            else
                itemsTask = collection.OfType<TInherited>().Find(session, match).Skip(page * pageSize).Limit(pageSize).ToListAsync();

            await Task.WhenAll(countTask, itemsTask);

            var items = itemsTask.Result;
            var count = countTask.Result;

            return new Paged<TInherited>
            {
                Items = items,
                ItemsPerPage = pageSize,
                Page = page + 1,
                Pages = (int)Math.Ceiling((double)count / pageSize),
                TotalItems = count,
            };
        }

        public virtual async Task<List<TInherited>> WhereAsync<TInherited>(Expression<Func<TInherited, bool>> match)
            where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(match)).ToListAsync();
            else
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(session, match)).ToListAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.FindAsync<T>(match)).SingleOrDefaultAsync();
            else
                return await (await collection.FindAsync<T>(session, match)).SingleOrDefaultAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.FindAsync<T>(match)).FirstOrDefaultAsync();
            else
                return await (await collection.FindAsync<T>(session, match)).FirstOrDefaultAsync();
        }

        public virtual async Task<TInherited> FirstOrDefaultAsync<TInherited>() where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var filter = Builders<TInherited>.Filter.Empty;

            if (session == null)
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(filter)).FirstOrDefaultAsync();
            else
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(session, filter)).FirstOrDefaultAsync();
        }

        public virtual async Task<TInherited> FirstOrDefaultAsync<TInherited>(Expression<Func<TInherited, bool>> match)
            where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(match)).FirstOrDefaultAsync();
            else
                return await (await collection.OfType<TInherited>().FindAsync<TInherited>(session, match)).FirstOrDefaultAsync();
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await (await collection.FindAsync<T>(match)).FirstAsync();
            else
                return await (await collection.FindAsync<T>(session, match)).FirstAsync();
        }

        public virtual async Task<List<TItem>> SelectAsync<TItem>(Expression<Func<T, TItem>> selector, Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            return await (session == null ? collection.AsQueryable() : collection.AsQueryable(session))
                .Where(match)
                .Select(selector)
                .ToListAsync();
        }

        public virtual async Task<List<TItem>> SelectAsync<TInherited, TItem>(Expression<Func<TInherited, TItem>> selector, Expression<Func<TInherited, bool>> match)
             where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var typeCollection = collection.OfType<TInherited>();

            return await (session == null ? typeCollection.AsQueryable() : typeCollection.AsQueryable(session))
                .Where(match)
                .Select(selector)
                .ToListAsync();
        }

        public virtual async Task<List<TItem>> SelectManyAsync<TItem>(Expression<Func<T, IEnumerable<TItem>>> selector, Expression<Func<TItem, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            return await (session == null ? collection.AsQueryable() : collection.AsQueryable(session))
                .SelectMany(selector)
                .Where(match)
                .ToListAsync();
        }

        public virtual async Task<Paged<TItem>> SelectManyPagedAsync<TItem>(Expression<Func<T, IEnumerable<TItem>>> selector, Expression<Func<TItem, bool>> childMatch, int page)
        {
            page = page > 0 ? page - 1 : 0;

            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var countTask = (session == null ? collection.AsQueryable() : collection.AsQueryable(session))
                   .SelectMany(selector)
                   .Where(childMatch)
                   .CountAsync();

            var pageSize = PageSize;

            var itemsTask = (session == null ? collection.AsQueryable() : collection.AsQueryable(session))
                   .SelectMany(selector)
                   .Where(childMatch)
                   .Skip(page * pageSize)
                   .Take(pageSize)
                   .ToListAsync();

            await Task.WhenAll(countTask, itemsTask);

            var items = itemsTask.Result;
            var count = countTask.Result;

            return new Paged<TItem>
            {
                Items = items,
                ItemsPerPage = pageSize,
                Page = page + 1,
                Pages = (int)Math.Ceiling((double)count / pageSize),
                TotalItems = count,
            };
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await collection.CountDocumentsAsync<T>(match);
            else
                return await collection.CountDocumentsAsync<T>(session, match);
        }

        public virtual async Task<long> CountAsync<TInherited>(Expression<Func<TInherited, bool>> match)
            where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await collection.OfType<TInherited>().CountDocumentsAsync(match);
            else
                return await collection.OfType<TInherited>().CountDocumentsAsync(session, match);
        }

        public virtual async Task<long> CountAsync()
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            if (session == null)
                return await collection.CountDocumentsAsync(FilterDefinition<T>.Empty);
            else
                return await collection.CountDocumentsAsync(session, FilterDefinition<T>.Empty);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            return await (session == null ? collection.AsQueryable() : collection.AsQueryable(session)).AnyAsync(match);
        }

        public virtual async Task<bool> AnyAsync<TInherited>(Expression<Func<TInherited, bool>> match)
            where TInherited : T
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var typeCollection = collection.OfType<TInherited>();

            return await (session == null ? typeCollection.AsQueryable() : typeCollection.AsQueryable(session))
                .AnyAsync(match);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            Guard.ThatIsFalse(id == Guid.Empty, nameof(id), nameof(ErrorCodeResource.Property_IncorrectValue));
            return await FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TInherited> GetByIdAsync<TInherited>(Guid id) where TInherited : T
        {
            Guard.ThatIsFalse(id == Guid.Empty, nameof(id), nameof(ErrorCodeResource.Property_IncorrectValue));
            return await FirstOrDefaultAsync<TInherited>(x => x.Id == id);
        }

        public async Task<List<T>> GetByIdsAsync(List<Guid> ids)
        {
            Guard.HasItems(ids, nameof(ids), nameof(ErrorCodeResource.Property_HasNoItems));
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            FieldDefinition<T, Guid> idFieldDefinition = "_id";
            var inFilter = Builders<T>.Filter.In(idFieldDefinition, ids);

            if (session == null)
                return await (await collection.FindAsync<T>(inFilter)).ToListAsync();
            else
                return await (await collection.FindAsync<T>(session, inFilter)).ToListAsync();
        }

        public async Task<List<TInherited>> GetByIdsAsync<TInherited>(List<Guid> ids) where TInherited : T
        {
            Guard.HasItems(ids, nameof(ids), nameof(ErrorCodeResource.Property_HasNoItems));
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var inFilter = Builders<TInherited>.Filter.In(f => f.Id, ids);

            if (session == null)
                return await (await collection.OfType<TInherited>().FindAsync(inFilter)).ToListAsync();
            else
                return await (await collection.OfType<TInherited>().FindAsync(session, inFilter)).ToListAsync();
        }

        public async Task<TProperty> GetMaxValueAsync<TProperty>(Expression<Func<T, TProperty>> selector, Expression<Func<T, bool>> match)
        {
            var collection = await MongoDataContext.GetCollectionAsync();
            var session = MongoDataContext.GetSession();

            var result = await (session == null ? collection.AsQueryable() : collection.AsQueryable(session))
                .Where(match)
                .OrderByDescending(selector)
                .Take(1)
                .FirstOrDefaultAsync();

            if (result == null)
                return default;

            return selector.Compile().Invoke(result);
        }
    }
}
