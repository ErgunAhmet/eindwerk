using Domain.Entities;
using Domain.Entities.Base;
using IInfrasctructure.MongoDB;

namespace IRepository
{
    public interface IAuditLogRepository : IMongoDBReadWriteRepository<AuditLog>
    {
        Task RemoveAllForEntityAsync(IEntity entity);
        Task RemoveAllForEntitiesAsync(List<IEntity> entities);
    }
}
