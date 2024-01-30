using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IInfrasctructure.MongoDB
{
    public interface IMongoDBReadWriteRepository<T> : IMongoDBReadRepository<T> where T : class, IEntity
    {
        Task InsertAsync(T obj);
        Task BatchInsertAsync(IEnumerable<T> obj);
        Task UpdateAsync(T obj);
        Task BatchUpdateByIdAsync(IEnumerable<T> obj);
        Task RemoveAsync(T obj);
        Task RemoveAllAsync(Expression<Func<T, bool>> match);
    }
}
