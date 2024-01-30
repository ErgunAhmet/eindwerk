using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    public abstract class RootEntity : IEntity
    {
        [BsonId]
        public Guid Id { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public DateTime UpdatedOn { get; protected set; }

        public virtual string GetAuditLogId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return GetAuditLogId();
        }
    }
}
