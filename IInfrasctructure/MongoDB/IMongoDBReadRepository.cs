using Domain.Entities.Base;
using Domain.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IInfrasctructure.MongoDB
{
    public interface IMongoDBReadRepository<T> : IMongoDBBaseRepository where T : class, IEntity
    {
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> match);
        Task<Paged<T>> WherePagedAsync(Expression<Func<T, bool>> match, int page);
        Task<Paged<TInherited>> WherePagedAsync<TInherited>(Expression<Func<TInherited, bool>> match, int page) where TInherited : T;
        Task<List<TInherited>> WhereAsync<TInherited>(Expression<Func<TInherited, bool>> match) where TInherited : T;
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> match);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match);
        Task<TInherited> FirstOrDefaultAsync<TInherited>() where TInherited : T;
        Task<TInherited> FirstOrDefaultAsync<TInherited>(Expression<Func<TInherited, bool>> match) where TInherited : T;
        Task<T> FirstAsync(Expression<Func<T, bool>> match);
        Task<long> CountAsync(Expression<Func<T, bool>> match);
        Task<long> CountAsync<TInherited>(Expression<Func<TInherited, bool>> match) where TInherited : T;
        Task<long> CountAsync();
        Task<List<TItem>> SelectAsync<TItem>(Expression<Func<T, TItem>> selector, Expression<Func<T, bool>> match);
        Task<List<TItem>> SelectAsync<TInherited, TItem>(Expression<Func<TInherited, TItem>> selector, Expression<Func<TInherited, bool>> match) where TInherited : T;
        Task<List<TItem>> SelectManyAsync<TItem>(Expression<Func<T, IEnumerable<TItem>>> selector, Expression<Func<TItem, bool>> match);
        Task<Paged<TItem>> SelectManyPagedAsync<TItem>(Expression<Func<T, IEnumerable<TItem>>> selector, Expression<Func<TItem, bool>> childMatch, int page);
        Task<T> GetByIdAsync(Guid id);
        Task<TInherited> GetByIdAsync<TInherited>(Guid id) where TInherited : T;
        Task<List<T>> GetByIdsAsync(List<Guid> ids);
        Task<List<TInherited>> GetByIdsAsync<TInherited>(List<Guid> ids) where TInherited : T;
        Task<bool> AnyAsync(Expression<Func<T, bool>> match);
        Task<bool> AnyAsync<TInherited>(Expression<Func<TInherited, bool>> match) where TInherited : T;
        Task<TProperty> GetMaxValueAsync<TProperty>(Expression<Func<T, TProperty>> selector, Expression<Func<T, bool>> match);
    }
}
