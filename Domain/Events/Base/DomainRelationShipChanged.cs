using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Base
{
    public class DomainRelationShipChanged
    {
        public Guid ObjectId { get; private set; }
        public string Name { get; private set; }
        public string OldObjectId { get; private set; }
        public string NewObjectId { get; private set; }
        public AuditLogAction Action { get; private set; }

        private DomainRelationShipChanged() { }

        public static DomainRelationShipChanged Create(IEntity obj, string name, IEntity oldObject, IEntity newObject)
        {
            AuditLogAction action;

            if (oldObject == null)
                action = AuditLogAction.Create;
            else if (newObject == null)
                action = AuditLogAction.Delete;
            else
                action = AuditLogAction.Update;

            return new DomainRelationShipChanged
            {
                ObjectId = obj.Id,
                Name = name,
                OldObjectId = oldObject?.GetAuditLogId(),
                NewObjectId = newObject?.GetAuditLogId(),
                Action = action
            };
        }
    }
}
