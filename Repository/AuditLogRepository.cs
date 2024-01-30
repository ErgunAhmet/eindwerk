using Domain.Entities;
using Domain.Entities.Base;
using Infrastructure.MongoDB;
using IRepository;
using Shared.Validation;
using Domain.Resources;

namespace Repository
{
    public class AuditLogRepository : MongoDBReadWriteRepository<AuditLog>, IAuditLogRepository
    {
        public async Task RemoveAllForEntitiesAsync(List<IEntity> entities)
        {
            Guard.HasItems(entities, nameof(entities), nameof(ErrorCodeResource.Property_HasNoItems));
            var objectIds = entities.Select(x => x.Id).Distinct().ToList();
            await RemoveAllAsync(x => objectIds.Contains(x.ObjectId));
        }

        public async Task RemoveAllForEntityAsync(IEntity entity)
        {
            Guard.AgainstNull(entity, nameof(entity), nameof(ErrorCodeResource.Property_IsNull));
            await RemoveAllAsync(x => x.ObjectId == entity.Id);
        }

    }
}
