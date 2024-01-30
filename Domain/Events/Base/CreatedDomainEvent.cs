using Domain.Entities.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Base
{
    public abstract class CreatedDomainEvent<T> : BaseDomainEvent where T : IEntity
    {
        public Guid ObjectId { get; private set; }
        public string RelatedObjectId { get; private set; }
        public string ObjectName { get; private set; }
        public string ObjectProperty { get; private set; }

        public CreatedDomainEvent(T obj, string updatedBy, string objectProperty)
            : this(obj, updatedBy, null, objectProperty)
        {
        }

        protected CreatedDomainEvent(T obj, string updatedBy, IEntity relatedObj, string objectProperty)
            : base(updatedBy, DateTime.UtcNow)
        {
            ObjectName = typeof(T).Name;
            ObjectProperty = objectProperty;
            ObjectId = obj.Id;
            RelatedObjectId = relatedObj?.GetAuditLogId();
        }

        public override IEnumerable<AuditLog> ToAuditLogs()
        {
            var auditLog = AuditLog.CreateFromCreatedEvent(this);

            var auditLogs = new List<AuditLog>();

            if (auditLog != null)
                auditLogs.Add(auditLog);

            return auditLogs;
        }
    }
}
