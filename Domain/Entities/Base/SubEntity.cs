using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    public abstract class SubEntity : IEntity
    {
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
