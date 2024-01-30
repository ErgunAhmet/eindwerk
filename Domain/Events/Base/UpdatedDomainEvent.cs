using Domain.Entities.Base;
using Domain.Entities;
using Domain.Resources;
using Shared.Validation;
using Domain.Extensions;
using System.Text.Json;

namespace Domain.Events.Base
{
    public abstract class UpdatedDomainEvent<T> : BaseDomainEvent where T : IEntity
    {
        public Guid ObjectId { get; private set; }
        public string ObjectName { get; private set; }
        public List<DomainPropertyUpdate> Updates { get; private set; }
        protected bool ForceUpdates { get; set; } = false;

        public UpdatedDomainEvent(T obj, string updatedBy)
            : this(obj, updatedBy, DateTime.UtcNow)
        {
        }

        public UpdatedDomainEvent(T obj, string updatedBy, DateTime updatedOn)
            : base(updatedBy, updatedOn)
        {
            Updates = new List<DomainPropertyUpdate>();

            ObjectName = typeof(T).Name;
            ObjectId = obj.Id;
        }

        protected void AddPropertyUpdated(string propertyName, IEntity oldEntity, IEntity newEntity)
        {
            AddPropertyUpdated(propertyName, oldEntity?.GetAuditLogId(), newEntity?.GetAuditLogId());
        }

        protected void AddPropertyUpdated(string propertyName, DateTime? oldDate, DateTime? newDate)
        {
            AddPropertyUpdated(propertyName, oldDate?.ToJsonString(), newDate?.ToJsonString());
        }

        protected void AddPropertyUpdated(string propertyName, bool? oldBool, bool? newBool)
        {
            AddPropertyUpdated(propertyName, oldBool?.GetResourceKey(), newBool?.GetResourceKey());
        }

        protected void AddPropertyUpdated(string propertyName, int? oldInt, int? newInt)
        {
            AddPropertyUpdated(propertyName, oldInt?.ToString(), newInt?.ToString());
        }

        protected void AddPropertyUpdated(string propertyName, double? oldDouble, double? newDouble)
        {
            AddPropertyUpdated(propertyName, oldDouble?.ToString(), newDouble?.ToString());
        }

        protected void AddPropertyUpdated(string propertyName, decimal? oldDecimal, decimal? newDecimal)
        {
            AddPropertyUpdated(propertyName, oldDecimal?.ToString(), newDecimal?.ToString());
        }

        protected void AddPropertyUpdated(string propertyName, string oldValue, string newValue)
        {
            Guard.AgainstNullOrEmpty(propertyName, nameof(propertyName), nameof(ErrorCodeResource.Property_IsNullOrEmpty));

            if (oldValue != newValue || ForceUpdates)
                Updates.Add(DomainPropertyUpdate.Create(propertyName, oldValue, newValue));
        }

        public override IEnumerable<AuditLog> ToAuditLogs()
        {
            return AuditLog.CreateFromUpdatedEvent(this) ?? new List<AuditLog>();
        }
    }
}
