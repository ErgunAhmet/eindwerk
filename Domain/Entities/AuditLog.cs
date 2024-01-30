using Domain.Entities.Base;
using Domain.Enums;
using Domain.Events.Base;
using Domain.Resources;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Validation;

namespace Domain.Entities
{
    public class AuditLog : RootEntity
    {
        [BsonRepresentation(BsonType.String)]
        public AuditLogType AuditType { get; private set; }
        [BsonRepresentation(BsonType.String)]
        public AuditLogAction Action { get; private set; }
        public Guid ObjectId { get; private set; }
        public string ObjectType { get; private set; }
        public string ObjectProperty { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public string UpdatedBy { get; private set; }

        private AuditLog() { }

        public static AuditLog CreateAttributeChanged(Guid objectId,
            string objectType,
            string objectProperty,
            string oldValue,
            string newValue,
            string updatedBy,
            DateTime updatedOn)
        {
            Guard.ThatIsFalse(objectId == Guid.Empty, nameof(objectId), nameof(ErrorCodeResource.Property_IncorrectValue));
            Guard.AgainstNullOrEmpty(objectType, nameof(objectType), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            Guard.AgainstNullOrEmpty(objectProperty, nameof(objectProperty), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            Guard.ThatIsTrue(updatedOn.Kind == DateTimeKind.Utc, nameof(updatedOn), nameof(ErrorCodeResource.Property_IncorrectValue));

            return new AuditLog
            {
                Id = Guid.NewGuid(),
                CreatedOn = updatedOn,
                AuditType = AuditLogType.Attribute,
                ObjectId = objectId,
                ObjectType = objectType,
                ObjectProperty = objectProperty,
                Action = AuditLogAction.Update,
                OldValue = oldValue,
                NewValue = newValue,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
        }

        public static AuditLog CreateRelationshipChanged(Guid objectId,
            string objectType,
            string objectProperty,
            AuditLogAction action,
            string oldRelationshipId,
            string newRelationshipId,
            string updatedBy,
            DateTime updatedOn)
        {
            Guard.ThatIsFalse(objectId == Guid.Empty, nameof(objectId), nameof(ErrorCodeResource.Property_IncorrectValue));
            Guard.AgainstNullOrEmpty(objectType, nameof(objectType), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            Guard.ThatIsFalse(action == AuditLogAction.None, nameof(action), nameof(ErrorCodeResource.Property_IncorrectValue));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            Guard.ThatIsTrue(updatedOn.Kind == DateTimeKind.Utc, nameof(updatedOn), nameof(ErrorCodeResource.Property_IncorrectValue));

            return new AuditLog
            {
                Id = Guid.NewGuid(),
                CreatedOn = updatedOn,
                AuditType = AuditLogType.Relationship,
                ObjectId = objectId,
                ObjectType = objectType,
                ObjectProperty = objectProperty,
                Action = action,
                OldValue = oldRelationshipId,
                NewValue = newRelationshipId,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
        }

        public static AuditLog CreateFromCreatedEvent<T>(CreatedDomainEvent<T> createdEvent) where T : IEntity
        {
            return CreateRelationshipChanged(createdEvent.ObjectId,
                createdEvent.ObjectName,
                createdEvent.ObjectProperty,
                AuditLogAction.Create,
                null,
                createdEvent.RelatedObjectId,
            createdEvent.UpdatedBy,
            createdEvent.UpdatedOn);
        }

        public static IEnumerable<AuditLog> CreateFromUpdatedEvent<T>(UpdatedDomainEvent<T> updatedEvent) where T : IEntity
        {
            if (updatedEvent.Updates?.Any() != true)
                yield break;

            foreach (var update in updatedEvent.Updates)
                yield return CreateAttributeChanged(updatedEvent.ObjectId,
                    updatedEvent.ObjectName,
                    update.Name,
                    update.OldValue,
                    update.NewValue,
                updatedEvent.UpdatedBy,
                updatedEvent.UpdatedOn);
        }

        public static IEnumerable<AuditLog> CreateFromRelationshipChangedEvent<T>(RelationshipChangedDomainEvent<T> relationshipChangedEvent) where T : IEntity
        {
            if (relationshipChangedEvent.Changes?.Any() != true)
                yield break;

            foreach (var change in relationshipChangedEvent.Changes)
            {
                yield return new AuditLog
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = relationshipChangedEvent.UpdatedOn,
                    AuditType = AuditLogType.Relationship,
                    ObjectId = change.ObjectId,
                    ObjectType = relationshipChangedEvent.ObjectName,
                    ObjectProperty = change.Name,
                    Action = change.Action,
                    OldValue = change.OldObjectId,
                    NewValue = change.NewObjectId,
                    UpdatedBy = relationshipChangedEvent.UpdatedBy,
                    UpdatedOn = relationshipChangedEvent.UpdatedOn
                };
            }
        }

        public void UpdateUpdatedBy(string updatedBy)
        {
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy), nameof(ErrorCodeResource.Property_IsNullOrEmpty));
            UpdatedBy = updatedBy;
        }

        public void Update(string objectProperty, string oldValue, string newValue)
        {
            ObjectProperty = objectProperty;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
