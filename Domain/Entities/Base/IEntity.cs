using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
        DateTime UpdatedOn { get; }

        string GetAuditLogId();
    }
}
